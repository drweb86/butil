plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("org.jetbrains.kotlin.plugin.compose")
}

android {
    namespace = "com.siarheikuchuk.butil"
    compileSdk = 36
    buildToolsVersion = "36.0.0"

    defaultConfig {
        applicationId = "com.siarheikuchuk.butil"
        minSdk = 23
        targetSdk = 36
        val defaultVersionCode = 20261008
        val defaultVersionName = "2026.10.08"
        versionCode = findProperty("appVersionCode")?.toString()?.toIntOrNull() ?: defaultVersionCode
        versionName = findProperty("appVersionName")?.toString()?.takeIf { it.isNotBlank() } ?: defaultVersionName
    }

    signingConfigs {
        val keystoreFile = System.getenv("ANDROID_KEYSTORE_FILE")
        if (!keystoreFile.isNullOrBlank()) {
            create("release") {
                storeFile = file(keystoreFile)
                storePassword = System.getenv("ANDROID_KEYSTORE_PASSWORD") ?: ""
                keyAlias = System.getenv("ANDROID_KEY_ALIAS") ?: ""
                keyPassword = System.getenv("ANDROID_KEY_PASSWORD") ?: ""
                enableV1Signing = true
                enableV2Signing = true
            }
        }
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            signingConfig = signingConfigs.findByName("release")
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }
    kotlinOptions {
        jvmTarget = "17"
    }
    buildFeatures {
        compose = true
    }

    // F-Droid
    dependenciesInfo {
        includeInApk = false
        includeInBundle = false
    }
}

// F-Droid: AGP ArtProfile output (assets/dexopt/baseline.prof{,m}) is not
// bit-reproducible across builders, so signature copying fails.
tasks.whenTaskAdded {
    if (name.contains("ArtProfile")) {
        enabled = false
    }
}

dependencies {
    val composeBom = platform("androidx.compose:compose-bom:2024.10.01")
    implementation(composeBom)
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    implementation("androidx.activity:activity-compose:1.9.3")
    debugImplementation("androidx.compose.ui:ui-tooling")
}
