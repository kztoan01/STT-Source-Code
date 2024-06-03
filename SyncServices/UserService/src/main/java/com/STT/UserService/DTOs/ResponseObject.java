package com.STT.UserService.DTOs;


import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.Data;

import java.util.ArrayList;
import java.util.List;

@Data
@JsonIgnoreProperties(ignoreUnknown = true)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class ResponseObject<T> {
    private int statusCode;
    private List<ErrorDTO> errors = new ArrayList<>();
    private String message;
    private boolean isChangeSuccessfully;
    private T data;
    private String ExpirationTime;
    private String token;
    private String RefreshToken;
}
