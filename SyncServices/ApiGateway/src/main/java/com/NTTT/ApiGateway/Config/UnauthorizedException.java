package com.NTTT.ApiGateway.Config;

public class UnauthorizedException extends RuntimeException {
    public UnauthorizedException() {
        super("Unauthorized access!");
    }
}

