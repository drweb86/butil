package com.siarheikuchuk.butil.ui

import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.runtime.CompositionLocalProvider
import androidx.compose.runtime.Immutable
import androidx.compose.runtime.staticCompositionLocalOf
import androidx.compose.ui.graphics.Color

private val DarkBackground = Color(0xFF0F1419)
private val DarkSurface = Color(0xFF252B37)
private val DarkAccent = Color(0xFF00D9FF)
private val DarkText = Color(0xFFE8EAED)
private val DarkError = Color(0xFFDE6259)
private val DarkSuccess = Color(0xFF81C784)
private val DarkWarning = Color(0xFFFBBF24)

private val LightBackground = Color(0xFFF3F3F3)
private val LightSurface = Color(0xFFFFFFFF)
private val LightAccent = Color(0xFF005A9E)
private val LightText = Color(0xFF000000)
private val LightError = Color(0xFFDA3B01)
private val LightSuccess = Color(0xFF4CAF50)
private val LightWarning = Color(0xFFB45309)

@Immutable
internal data class StatusColors(val success: Color, val warning: Color)

internal val LocalStatusColors = staticCompositionLocalOf {
    StatusColors(success = LightSuccess, warning = LightWarning)
}

@Composable
fun BUtilTheme(content: @Composable () -> Unit) {
    val dark = isSystemInDarkTheme()
    val colors = if (dark) {
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
    val statusColors = if (dark) {
        StatusColors(success = DarkSuccess, warning = DarkWarning)
    } else {
        StatusColors(success = LightSuccess, warning = LightWarning)
    }
    CompositionLocalProvider(LocalStatusColors provides statusColors) {
        MaterialTheme(colorScheme = colors, content = content)
    }
}
