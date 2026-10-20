package com.siarheikuchuk.butil.core.services;

import java.io.IOException;

/** AES-256 decryption rejected the password. The output must not be kept. */
public final class InvalidPasswordException extends IOException {
    public InvalidPasswordException(Throwable cause) {
        super("The password is incorrect.", cause);
    }
}
