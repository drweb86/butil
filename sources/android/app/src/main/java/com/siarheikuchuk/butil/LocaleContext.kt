package com.siarheikuchuk.butil

import android.content.Context
import android.content.res.Configuration
import java.util.Locale

fun Context.withEnglishLocale(): Context {
    val locale = Locale.US
    Locale.setDefault(locale)
    val config = Configuration(resources.configuration)
    config.setLocale(locale)
    return createConfigurationContext(config)
}
