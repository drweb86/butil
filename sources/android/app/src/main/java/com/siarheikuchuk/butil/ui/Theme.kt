package com.siarheikuchuk.butil.ui

import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color

private val DarkBackground = Color(0xFF0F1419)
private val DarkSurface = Color(0xFF252B37)
private val DarkAccent = Color(0xFF00D9FF)
private val DarkText = Color(0xFFE8EAED)
private val DarkError = Color(0xFFDE6259)

private val LightBackground = Color(0xFFF3F3F3)
private val LightSurface = Color(0xFFFFFFFF)
private val LightAccent = Color(0xFF005A9E)
private val LightText = Color(0xFF000000)
private val LightError = Color(0xFFDA3B01)

@Composable
fun BUtilTheme(content: @Composable () -> Unit) {
    val colors = if (isSystemInDarkTheme()) {
        darkColorScheme(
            primary = DarkAccent,
            onPrimary = DarkBackground,
            background = DarkBackground,
            surface = DarkSurface,
            onBackground = DarkText,
            onSurface = DarkText,
            error = DarkError,
            onError = DarkBackground,
        )
    } else {
        lightColorScheme(
            primary = LightAccent,
            onPrimary = LightSurface,
            background = LightBackground,
            surface = LightSurface,
            onBackground = LightText,
            onSurface = LightText,
            error = LightError,
            onError = LightSurface,
        )
    }
    MaterialTheme(colorScheme = colors, content = content)
}
