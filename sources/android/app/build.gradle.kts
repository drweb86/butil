plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.plugin.compose")
}

android {
    namespace = "com.siarheikuchuk.butil"
    compileSdk = 37
    buildToolsVersion = "36.0.0"

    defaultConfig {
        applicationId = "com.siarheikuchuk.butil"
        minSdk = 23
        targetSdk = 36
        val defaultVersionCode = 20261105
        val defaultVersionName = "2026.11.05"
        versionCode = findProperty("appVersionCode")?.toString()?.toIntOrNull() ?: defaultVersionCode
        versionName = findProperty("appVersionName")?.toString()?.takeIf { it.isNotBlank() } ?: defaultVersionName
        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    flavorDimensions += "policy"
    productFlavors {
        create("general") {
            dimension = "policy"
            isDefault = true
            buildConfigField("boolean", "CHINA_PIPL_POLICY", "false")
        }
        create("chinaPiplPolicy") {
            dimension = "policy"
            buildConfigField("boolean", "CHINA_PIPL_POLICY", "true")
        }
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
    buildFeatures {
        compose = true
        buildConfig = true
    }

    testOptions {
        unitTests.isIncludeAndroidResources = true
    }

    // F-Droid
    dependenciesInfo {
        includeInApk = false
        includeInBundle = false
    }

    packaging {
        resources {
            excludes += "/META-INF/{AL2.0,LGPL2.1}"
            excludes += "META-INF/LICENSE.md"
            excludes += "META-INF/NOTICE.md"
            excludes += "META-INF/versions/9/OSGI-INF/MANIFEST.MF"
            pickFirsts += "META-INF/versions/9/OSGI-INF/MANIFEST.MF"
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

abstract class CopyPrivacyPoliciesTask : DefaultTask() {
    @get:InputDirectory
    @get:PathSensitive(PathSensitivity.RELATIVE)
    abstract val sourceDir: DirectoryProperty

    @get:OutputDirectory
    abstract val outputDir: DirectoryProperty

    @TaskAction
    fun copyPolicies() {
        copyMdFiles(sourceDir.get().asFile, outputDir.get().asFile.resolve("privacy"))
    }

    private fun copyMdFiles(source: java.io.File, dest: java.io.File) {
        dest.deleteRecursively()
        dest.mkdirs()
        source.listFiles()
            ?.filter { it.isFile && it.extension.equals("md", ignoreCase = true) && !it.name.equals("README.md", ignoreCase = true) }
            ?.forEach { it.copyTo(dest.resolve(it.name), overwrite = true) }
    }
}

abstract class CopyThirdPartyNoticesTask : DefaultTask() {
    @get:InputFile
    @get:PathSensitive(PathSensitivity.RELATIVE)
    abstract val sourceFile: RegularFileProperty

    @get:InputFile
    @get:PathSensitive(PathSensitivity.RELATIVE)
    abstract val creditsFile: RegularFileProperty

    @get:OutputDirectory
    abstract val outputDir: DirectoryProperty

    @TaskAction
    fun copyNotice() {
        val dest = outputDir.get().asFile
        dest.mkdirs()
        sourceFile.get().asFile.copyTo(dest.resolve("THIRD-PARTY-NOTICES.md"), overwrite = true)
        creditsFile.get().asFile.copyTo(dest.resolve("CREDITS.md"), overwrite = true)
    }
}

abstract class CopyLicenseDocumentsTask : DefaultTask() {
    @get:InputDirectory
    @get:PathSensitive(PathSensitivity.RELATIVE)
    abstract val sourceDir: DirectoryProperty

    @get:InputDirectory
    @get:PathSensitive(PathSensitivity.RELATIVE)
    abstract val appendixDir: DirectoryProperty

    @get:OutputDirectory
    abstract val outputDir: DirectoryProperty

    @TaskAction
    fun copyLicenses() {
        copyMdFiles(sourceDir.get().asFile, outputDir.get().asFile.resolve("license"))
        copyMdFiles(appendixDir.get().asFile, outputDir.get().asFile.resolve("license/china-appendix"))
    }

    private fun copyMdFiles(source: java.io.File, dest: java.io.File) {
        dest.deleteRecursively()
        dest.mkdirs()
        source.listFiles()
            ?.filter { it.isFile && it.extension.equals("md", ignoreCase = true) && !it.name.equals("README.md", ignoreCase = true) }
            ?.forEach { it.copyTo(dest.resolve(it.name), overwrite = true) }
    }
}

androidComponents {
    beforeVariants { builder ->
        builder.hostTests[com.android.build.api.variant.HostTestBuilder.UNIT_TEST_TYPE]?.enable = true
    }
    onVariants { variant ->
        val copyPrivacyPolicies = tasks.register<CopyPrivacyPoliciesTask>(
            "copyPrivacyPolicies${variant.name.replaceFirstChar { it.uppercase() }}",
        ) {
            sourceDir.set(rootProject.projectDir.resolve("../../privacy/android"))
        }
        val copyThirdPartyNotices = tasks.register<CopyThirdPartyNoticesTask>(
            "copyThirdPartyNotices${variant.name.replaceFirstChar { it.uppercase() }}",
        ) {
            sourceFile.set(rootProject.projectDir.resolve("THIRD-PARTY-NOTICES.md"))
            creditsFile.set(rootProject.projectDir.resolve("../../CREDITS.md"))
        }
        val copyLicenses = tasks.register<CopyLicenseDocumentsTask>(
            "copyLicenses${variant.name.replaceFirstChar { it.uppercase() }}",
        ) {
            sourceDir.set(rootProject.projectDir.resolve("../../licenses"))
            appendixDir.set(rootProject.projectDir.resolve("../../licenses/china-appendix"))
        }
        variant.sources.assets?.addGeneratedSourceDirectory(
            copyPrivacyPolicies,
            CopyPrivacyPoliciesTask::outputDir,
        )
        variant.sources.assets?.addGeneratedSourceDirectory(
            copyThirdPartyNotices,
            CopyThirdPartyNoticesTask::outputDir,
        )
        variant.sources.assets?.addGeneratedSourceDirectory(
            copyLicenses,
            CopyLicenseDocumentsTask::outputDir,
        )
        if (variant.flavorName == "chinaPiplPolicy" && variant.buildType == "debug") {
            variant.applicationId.set("com.siarheikuchuk.butil.chinapipl.debug")
        }
    }
}

kotlin {
    compilerOptions {
        jvmTarget.set(org.jetbrains.kotlin.gradle.dsl.JvmTarget.JVM_17)
    }
}

dependencies {
    coreLibraryDesugaring("com.android.tools:desugar_jdk_libs:2.1.5")
    val composeBom = platform("androidx.compose:compose-bom:2026.09.00")
    implementation(composeBom)
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    implementation("androidx.compose.material:material-icons-core")
    implementation("androidx.activity:activity-compose:1.13.0")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.11.0")
    implementation("org.brotli:dec:0.1.2")
    implementation("org.bouncycastle:bcprov-jdk18on:1.86")
    implementation("org.bouncycastle:bcpkix-jdk18on:1.86")
    implementation("androidx.documentfile:documentfile:1.1.0")
    implementation("androidx.core:core-ktx:1.19.1")
    implementation("commons-net:commons-net:3.13.0")
    implementation("org.conscrypt:conscrypt-android:2.8.0")
    implementation("com.hierynomus:sshj:0.41.2")
    implementation("org.slf4j:slf4j-nop:2.0.20")
    implementation("com.hierynomus:smbj:0.15.0")
    debugImplementation("androidx.compose.ui:ui-tooling")
    androidTestImplementation("androidx.test.ext:junit:1.3.0")
    androidTestImplementation("androidx.test:runner:1.7.0")
    androidTestImplementation("androidx.test:core:1.7.0")
}
