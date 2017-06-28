using System;
using BUtil.Core.Misc;
using BUtil.Core.FileSystem;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

using BULocalization;
// related documents:
// 7-zip protection
// Deploying sources

[assembly: CLSCompliant(true)]
namespace BUtil.Tools.Md5
{
	class MainClass
	{
        private const string _COPYRIGHT = "BUtil toolkit - Tool for signing files with md5 checksumm, (c) 2007-2009 BUtil project\n";
        private const string _DevelopmentCommand = "DEV";
        private const string _MD5Command = "MD5";
        private const string _VerifyCommand = "VERIFY";
        private const string _SignCommand = "SIGN";
        private const string _HelpCommand = "HELP";

        private static string _Usage;
        //Please, enter command: 
        private static string _EnterCommand;
        //Would you like to enter required arguments in interactive mode?
        private static string _AskEnterArguments;
        //File '{0}':\nmd5: {1}\nOld md5: {2}\n
        private static string _Md5OfPackerFormatString;
        //Operation failled:\n{0}
        private static string _OperationFailledFormatString;
        //Enter Parameter 1(or just <Enter> if there's no parameter 1 in command): 
        private static string _EnterParam1;
        //Enter Parameter 2(or just <Enter> if there's no parameter 2 in command): 
        private static string _EnterParam2;
        //CORRECT\n
        private static string _Ok;
        //Does NOT match\n
        private static string _Bad;
        //MD5Signer - commands
        private static string _Md5Commands;
        //MD5Signer - Question
        private static string _Md5Question;
        //You didn't specified parameter(s)
        private static string _NoParameters;
        //Operation not supported
        private static string _InvalidOperation;
        //Input file wasn't specified
        private static string _InputFileNotSpecified;
        //Press any key to quit
        private static string _PressKeyToQuit;
        
        private static void showHelp()
        {
        	MessageBox.Show(_Usage, _Md5Commands, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
        }
        
        private static void showMd5OfPacker()
        {
            Console.WriteLine(_Md5OfPackerFormatString, Files.SevenZipPacker, MD5Class.GetFileMD5(Files.SevenZipPacker), Files.Packer7ZipExeMd5);
            Console.WriteLine(_Md5OfPackerFormatString, Files.SevenZipGPacker, MD5Class.GetFileMD5(Files.SevenZipGPacker), Files.Packer7ZipGExeMd5);
            Console.WriteLine(_Md5OfPackerFormatString, Files.SevenZipPackerDll, MD5Class.GetFileMD5(Files.SevenZipPackerDll), Files.Packer7ZipDllMd5);
        }

        private static void showMd5(string file)
        {
            if (!string.IsNullOrEmpty(file))
                Console.WriteLine(MD5Class.GetFileMD5(file));
            else
                Console.WriteLine(_InputFileNotSpecified);
        }

        private static void verifyMd5(string file, string fileWithMd5)
        {
            if (!string.IsNullOrEmpty(file))
            {
                string srcMd5 = MD5Class.GetFileMD5(file);
                bool result = (File.ReadAllText(fileWithMd5) == srcMd5);
                if (result)
                    Console.WriteLine(_Ok);
                else
                    Console.WriteLine(_Bad);
            }
            else
                Console.WriteLine(_InputFileNotSpecified);
        
        }

        private static void sign(string file, string md5File)
        {
            try
            {
                if (!string.IsNullOrEmpty(file))
                {
                    if (File.Exists(md5File))
                    {
                        File.Delete(md5File);
                    }

                    string srcMd5 = MD5Class.GetFileMD5(file);
                    File.WriteAllText(md5File, srcMd5);
                }
                else
                {
                    Console.WriteLine(_InputFileNotSpecified);
                }
            }
            catch
            {
                Console.Beep();
                throw;
            }
        }

        private static string stripQuote(string sourceString)
        {
        	if (sourceString.Length > 0)
        	{
        		if (sourceString[0] == '\"')
        		{
        			sourceString = sourceString.Remove(0, 1);
        		}
        	}
        	
        	if (sourceString.Length > 0)
        	{
        		if (sourceString[sourceString.Length - 1] == '\"')
        		{
        			sourceString = sourceString.Remove(sourceString.Length - 1, 1);
        		}
        	}
        	
        	return sourceString;
        }

        private static void main(string[] args)
        {
            if (args.Length > 0)
            {
                string operation = args[0];
                string srcFile;
                string md5File;

                if (args.Length == 1)
                {
                    srcFile = string.Empty;
                }
                else
                {
                    srcFile = args[1];
                }

                if (args.Length == 3)
                {
                    md5File = args[2];
                }
                else
                {
                    md5File = srcFile + ".md5";
                }

                try
                {
                    switch (operation.ToUpperInvariant())
                    {
                        case _DevelopmentCommand:
                            showMd5OfPacker();
                            break;

                        case _MD5Command:
                            showMd5(srcFile);
                            break;

                        case _VerifyCommand:
                            verifyMd5(srcFile, md5File);
                            break;

                        case _SignCommand:
                            sign(srcFile, md5File);
                            break;

                        case _HelpCommand:
                            showHelp();
                            break;

                        default:
                            Console.WriteLine(_InvalidOperation);
                            showHelp();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(_OperationFailledFormatString, e.Message);
                    Environment.ExitCode = -1;
                }
            }
            else
            {
                Console.WriteLine(_NoParameters);
                showHelp();
                if (MessageBox.Show(_AskEnterArguments, _Md5Question, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 0) == DialogResult.Yes)
                {
                    List<string> parameters = new List<string>();

                    Console.Write(_EnterCommand);
                    string command = Console.ReadLine();
                    parameters.Add(command);

                    Console.WriteLine(_EnterParam1);
                    string param1 = Console.ReadLine();
                    if (!string.IsNullOrEmpty(param1))
                    {
                        parameters.Add(stripQuote(param1));

                        Console.WriteLine(_EnterParam2);
                        string param2 = Console.ReadLine();
                        if (!string.IsNullOrEmpty(stripQuote(param2)))
                        {
                            parameters.Add(stripQuote(param2));
                        }
                    }

                    main(parameters.ToArray());
                }
            }
        }


        [STAThread]
        public static void Main(string[] args)
        {
        	Environment.ExitCode = 0;
            ManagerBehaviorSettings settings = new ManagerBehaviorSettings();
            settings.RequestLanguageIfNotSpecified = true;
            settings.UseToolGeneratedConfigFile = true;
            LanguagesManager localManager = new LanguagesManager(new System.Collections.ObjectModel.ReadOnlyCollection<string>(new string[] { "md5 Program" }), Directories.LocalsFolder, "BUtil", settings);
            localManager.Init();
            localManager.Apply();

            // applying locals
			_Usage = Translation.Current[488];
			_EnterCommand = Translation.Current[489];
			_AskEnterArguments = Translation.Current[490];
			_Md5OfPackerFormatString = Translation.Current[491];
			_OperationFailledFormatString = Translation.Current[492];
			_EnterParam1 = Translation.Current[493];
			_EnterParam2 = Translation.Current[494];
			_Ok = Translation.Current[495];
			_Bad = Translation.Current[496];
			_Md5Commands = Translation.Current[497];
			_Md5Question = Translation.Current[498];
			_NoParameters = Translation.Current[499];
			_InvalidOperation = Translation.Current[500];
			_InputFileNotSpecified = Translation.Current[501];
			_PressKeyToQuit = Translation.Current[502];
            
            main(args);
            Console.WriteLine(_PressKeyToQuit);
			Console.ReadKey();
			Console.WriteLine(Environment.NewLine);
            Console.WriteLine(_COPYRIGHT);
        }
	}
}
