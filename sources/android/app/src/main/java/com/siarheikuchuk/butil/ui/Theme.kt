package com.siarheikuchuk.butil.ui

import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.graphics.Color

private val Background = Color(0xFF0F1419)
private val SurfaceColor = Color(0xFF252B37)
private val Accent = Color(0xFF00D9FF)
private val TextColor = Color(0xFFE8EAED)

@Composable
fun BUtilTheme(content: @Composable () -> Unit) {
    MaterialTheme(
        colorScheme = darkColorScheme(
            primary = Accent,
            onPrimary = Background,
            background = Background,
            surface = SurfaceColor,
            onBackground = TextColor,
            onSurface = TextColor,
        ),
        content = content,
    )
}
