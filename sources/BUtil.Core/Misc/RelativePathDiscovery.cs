#nullable disable
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BUtil.Core.Misc
{
    internal static class RelativePathDiscovery
    {
        /// <summary>
        /// Produces relative path when possible to go from baseLocation to targetLocation
        /// </summary>
        /// <param name="baseLocation">The root folder</param>
        /// <param name="targetLocation">The target folder</param>
        /// <returns>The relative path relative to baseLocation</returns>
        /// <exception cref="ArgumentNullException">base or target locations are null or empty</exception>
        public static string ProduceRelativePath(string baseLocation, string targetLocation)
        {
            if (string.IsNullOrEmpty(baseLocation))
            {
                throw new ArgumentNullException("baseLocation");
            }

            if (string.IsNullOrEmpty(targetLocation))
            {
                throw new ArgumentNullException("targetLocation");
            }

            if (!Path.IsPathRooted(baseLocation))
            {
                return baseLocation;
            }

            if (!Path.IsPathRooted(targetLocation))
            {
                return targetLocation;
            }

            if (string.Compare(Path.GetPathRoot(baseLocation), Path.GetPathRoot(targetLocation), true) != 0)
            {
                return targetLocation;
            }

            if (string.Compare(baseLocation, targetLocation, true) == 0)
            {
                return ".";
            }

            string resultPath = ".";

            if (!targetLocation.EndsWith(@"\"))
            {
                targetLocation = targetLocation + @"\";
            }

            if (baseLocation.EndsWith(@"\"))
            {
                baseLocation = baseLocation.Substring(0, baseLocation.Length - 1);
            }

            while (!targetLocation.StartsWith(baseLocation + @"\", StringComparison.OrdinalIgnoreCase))
            {
                resultPath = resultPath + @"\..";
                baseLocation = Path.GetDirectoryName(baseLocation);

                if (baseLocation.EndsWith(@"\"))
                {
                    baseLocation = baseLocation.Substring(0, baseLocation.Length - 1);
                }
            }

            resultPath = resultPath + targetLocation.Substring(baseLocation.Length);

            // preprocess .\ case
            return resultPath.Substring(2, resultPath.Length - 3);
        }

        /// <summary>
        /// Resolves the relative pathes
        /// </summary>
        /// <param name="relativePath">Relative path</param>
        /// <param name="basePath">base path for discovering</param>
        /// <returns>Resolved path</returns>
        public static string ResolveRelativePath(string relativePath, string basePath)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                throw new ArgumentNullException("basePath");
            }

            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("relativePath");
            }

            var result = basePath;

            if (Path.IsPathRooted(relativePath))
            {
                return relativePath;
            }

            if (relativePath.EndsWith(@"\"))
            {
                relativePath = relativePath.Substring(0, relativePath.Length - 1);
            }

            if (relativePath == ".")
            {
                return basePath;
            }

            if (relativePath.StartsWith(@".\"))
            {
                relativePath = relativePath.Substring(2);
            }

            relativePath = relativePath.Replace(@"\.\", @"\");
            if (!relativePath.EndsWith(@"\"))
            {
                relativePath = relativePath + @"\";
            }

            while (!string.IsNullOrEmpty(relativePath))
            {
                int lengthOfOperation = relativePath.IndexOf(@"\") + 1;
                var operation = relativePath.Substring(0, lengthOfOperation - 1);
                relativePath = relativePath.Remove(0, lengthOfOperation);

                if (operation == @"..")
                {
                    result = Path.GetDirectoryName(result);
                }
                else
                {
                    result = Path.Combine(result, operation);
                }
            }

            return result;
        }
    }
}