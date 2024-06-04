package com.NTTT.ApiGateway.Clients;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.Data;

import java.util.ArrayList;
import java.util.List;

@Data
@JsonIgnoreProperties(ignoreUnknown = true)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class ResponseObject {

    private int statusCode;
    private List<ErrorDTO> errors = new ArrayList<>();
    private String message;
    private String token;
    private String refreshToken;
    private String expirationTime;
    private ResponseUserDTO responseUserDTO;
    private boolean isChangeSuccessfully;
}
