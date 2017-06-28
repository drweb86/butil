

using System;
using NUnit.Framework;
using BUtil.Core;
using BUtil.Core.PL;

namespace NUnitTests
{
	[TestFixture]
	public class EncryptionControlTest
	{
		static string makeSequence(int length)
		{
			string result = string.Empty;
			
			while (length>0)
			{
				result += '1';
				length--;
			}

			return result;
		}
		
		[Test]
		public void PasswordChanged()
		{
			// with check for length
			if (EncryptionUserControl.Behaviour.PasswordChanged(string.Empty, false) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("1 - Empty");
			if (EncryptionUserControl.Behaviour.PasswordChanged(null, false) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("2 - Null");
			if (EncryptionUserControl.Behaviour.PasswordChanged(" ", false) != EncryptionUserControl.Result.PasswordContainsForbiddenCharacters)
				Assert.Fail("3 - Forbidden characters");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MinimumPasswordLength), false) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("4 - Minimum");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MaximumPasswordLength), false) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("5 - Maximum");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MinimumPasswordLength-1), false) != EncryptionUserControl.Result.PasswordHasInvalidSize)
				Assert.Fail("6 - Min-1");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MaximumPasswordLength+1), false) != EncryptionUserControl.Result.PasswordHasInvalidSize)
				Assert.Fail("7 - Max+1");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MaximumPasswordLength-1), false) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("8 - In Interval");
			
			// without
			if (EncryptionUserControl.Behaviour.PasswordChanged(string.Empty, true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("9 - Empty");
			if (EncryptionUserControl.Behaviour.PasswordChanged(null, true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("10 - Null");
			if (EncryptionUserControl.Behaviour.PasswordChanged(" ", true) != EncryptionUserControl.Result.PasswordContainsForbiddenCharacters)
				Assert.Fail("11 - Forbidden characters");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MinimumPasswordLength), true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("12 - Minimum");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MaximumPasswordLength), true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("13 - Maximum");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MinimumPasswordLength-1), true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("14 - Min-1");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MaximumPasswordLength+1), true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("15 - Max+1");
			if (EncryptionUserControl.Behaviour.PasswordChanged(makeSequence(Constants.MaximumPasswordLength-1), true) != EncryptionUserControl.Result.PasswordIsValid)
				Assert.Fail("16 - In Interval");
		}
		
		[Test]
		public void ConfirmationChanged()
		{
			if (EncryptionUserControl.Behaviour.ConfirmationOfPasswordChanged(string.Empty, string.Empty) != EncryptionUserControl.Result.ConfirmationOfPasswordIsValid)
				Assert.Fail("1 - Empty - Empty");
			if (EncryptionUserControl.Behaviour.ConfirmationOfPasswordChanged(string.Empty, null) != EncryptionUserControl.Result.ConfirmationOfPasswordIsValid)
				Assert.Fail("2 - Empty - Null");
			if (EncryptionUserControl.Behaviour.ConfirmationOfPasswordChanged("test", "test") != EncryptionUserControl.Result.ConfirmationOfPasswordIsValid)
				Assert.Fail("3 - test - test");
			if (EncryptionUserControl.Behaviour.ConfirmationOfPasswordChanged("test1", "test") != EncryptionUserControl.Result.ConfirmationIsNotEqualToPassword)
				Assert.Fail("4 - test1 - test");
			
		}
	}
}
