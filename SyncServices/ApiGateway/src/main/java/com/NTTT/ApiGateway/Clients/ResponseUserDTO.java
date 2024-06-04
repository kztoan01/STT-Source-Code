package com.NTTT.ApiGateway.Clients;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@Data
@AllArgsConstructor
@NoArgsConstructor
public class ResponseUserDTO {

    private String userId;

    private String firstName;


    private String lastName;


    private String phoneNumber;


    private String emailAddress;


    private String userName;

    private String facebook;


    private String apple;


    private Boolean activeStatus;


    private Integer userRole;
}
