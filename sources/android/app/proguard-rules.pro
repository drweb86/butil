# Used only by the play build type. The release APK, release AAB, and China AAB do not minify.
# proguard-android-optimize.txt keeps line numbers, so mapping.txt can restore stack traces.

# BouncyCastle registers digest and cipher classes by name.
-keep class org.bouncycastle.jcajce.provider.** { *; }
-keep class org.bouncycastle.jce.provider.** { *; }
-dontwarn org.bouncycastle.**

# SSHJ and SMBJ reference optional libraries that are not packaged.
# MBassador, used by SMBJ, can filter events with the Expression Language API.
-dontwarn net.i2p.crypto.eddsa.**
-dontwarn com.jcraft.jzlib.**
-dontwarn org.ietf.jgss.**
-dontwarn org.slf4j.impl.**
-dontwarn javax.el.**
