plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("org.jetbrains.kotlin.plugin.compose")
}

val thirdPartyNoticesDir = layout.buildDirectory.dir("generated/thirdPartyNotices")
val syncThirdPartyNotices = tasks.register<Copy>("syncThirdPartyNotices") {
    from(rootProject.file("THIRD-PARTY-NOTICES.md"))
    into(thirdPartyNoticesDir)
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
        isCoreLibraryDesugaringEnabled = true
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }
    kotlinOptions {
        jvmTarget = "17"
    }
    buildFeatures {
        compose = true
        buildConfig = true
    }
    sourceSets {
        getByName("main") {
            assets.srcDir(thirdPartyNoticesDir)
        }
    }

    // F-Droid
    dependenciesInfo {
        includeInApk = false
        includeInBundle = false
    }

    packaging {
        resources {
            excludes += "META-INF/versions/9/OSGI-INF/MANIFEST.MF"
        }
    }
}

// F-Droid: AGP ArtProfile output (assets/dexopt/baseline.prof{,m}) is not
// bit-reproducible across builders, so signature copying fails.
tasks.whenTaskAdded {
    if (name.contains("ArtProfile")) {
        enabled = false
    }
}

tasks.named("preBuild").configure {
    dependsOn(syncThirdPartyNotices)
}
tasks.configureEach {
    if (name.startsWith("merge") && name.endsWith("Assets")) {
        dependsOn(syncThirdPartyNotices)
    }
}

dependencies {
    coreLibraryDesugaring("com.android.tools:desugar_jdk_libs:2.1.4")
    val composeBom = platform("androidx.compose:compose-bom:2024.10.01")
    implementation(composeBom)
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    implementation("androidx.compose.material:material-icons-core")
    implementation("androidx.activity:activity-compose:1.9.3")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.9.0")
    implementation("org.brotli:dec:0.1.2")
    implementation("org.bouncycastle:bcpkix-jdk18on:1.79")
    implementation("androidx.documentfile:documentfile:1.0.1")
    implementation("androidx.core:core-ktx:1.13.1")
    implementation("commons-net:commons-net:3.11.1")
    implementation("com.hierynomus:sshj:0.39.0")
    implementation("org.slf4j:slf4j-nop:2.0.16")
    implementation("com.hierynomus:smbj:0.13.0")
    debugImplementation("androidx.compose.ui:ui-tooling")
}
