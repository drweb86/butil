package com.siarheikuchuk.butil

import android.os.Bundle
import android.os.Process
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import com.siarheikuchuk.butil.core.legal.AppPrivateDataWiper
import com.siarheikuchuk.butil.core.legal.LicenseStore
import com.siarheikuchuk.butil.core.legal.PrivacyStore
import com.siarheikuchuk.butil.ui.BUtilTheme
import com.siarheikuchuk.butil.ui.controls.LegalDocumentKind
import com.siarheikuchuk.butil.ui.controls.LegalDocumentScreen
import com.siarheikuchuk.butil.ui.controls.LegalScreenMode
import com.siarheikuchuk.butil.ui.controls.MainWindow

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            var consented by remember {
                mutableStateOf(!BuildConfig.CHINA_PIPL_POLICY || PrivacyStore.hasConsent(this@MainActivity))
            }
            var licenseConsented by remember {
                mutableStateOf(!BuildConfig.CHINA_PIPL_POLICY || LicenseStore.hasConsent(this@MainActivity))
            }
            var privacyOpen by remember { mutableStateOf(false) }
            var licenseOpen by remember { mutableStateOf(false) }
            BUtilTheme {
                val china = BuildConfig.CHINA_PIPL_POLICY
                val viewMode = if (china) LegalScreenMode.InfoWithWithdraw else LegalScreenMode.Info
                when {
                    china && !consented -> LegalDocumentScreen(
                        mode = LegalScreenMode.ConsentGate,
                        kind = LegalDocumentKind.Privacy,
                        includeChinaAppendix = false,
                        onDismiss = { },
                        onAgree = {
                            PrivacyStore.acceptConsent(this@MainActivity)
                            consented = true
                        },
                        onDeclineConfirmed = { wipePrivateDataAndExit() },
                    )
                    china && !licenseConsented -> LegalDocumentScreen(
                        mode = LegalScreenMode.ConsentGate,
                        kind = LegalDocumentKind.License,
                        includeChinaAppendix = true,
                        onDismiss = { },
                        onAgree = {
                            LicenseStore.acceptConsent(this@MainActivity)
                            licenseConsented = true
                        },
                        onDeclineConfirmed = { wipePrivateDataAndExit() },
                    )
                    privacyOpen -> LegalDocumentScreen(
                        mode = viewMode,
                        kind = LegalDocumentKind.Privacy,
                        includeChinaAppendix = false,
                        onDismiss = { privacyOpen = false },
                        onDeclineConfirmed = { wipePrivateDataAndExit() },
                    )
                    licenseOpen -> LegalDocumentScreen(
                        mode = viewMode,
                        kind = LegalDocumentKind.License,
                        includeChinaAppendix = china,
                        onDismiss = { licenseOpen = false },
                        onDeclineConfirmed = { wipePrivateDataAndExit() },
                    )
                    else -> MainWindow(
                        onOpenPrivacy = { privacyOpen = true },
                        onOpenLicense = { licenseOpen = true },
                    )
                }
            }
        }
    }

    private fun wipePrivateDataAndExit() {
        AppPrivateDataWiper.wipePrivateDataAndClearConsent(this)
        finishAffinity()
        Process.killProcess(Process.myPid())
    }
}
