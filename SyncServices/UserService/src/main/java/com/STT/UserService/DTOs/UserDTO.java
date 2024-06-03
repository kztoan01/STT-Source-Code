package com.STT.UserService.DTOs;

import com.STT.UserService.Model.Artists;
import com.STT.UserService.Model.Followers;
import jakarta.annotation.Nullable;
import jakarta.persistence.*;
import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.UuidGenerator;

import java.util.Date;
import java.util.List;
import java.util.UUID;


@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserDTO {


    private String country;

    @NotNull(message = "username must not be null")
    private String username;

    @NotNull(message = "password must not be null")
    private  String password;

    private String fullName;

    private Date birthDate;

    @NotNull(message = "email must not be null")
    private  String emailAddress;

    @NotNull(message = "role must not be null")
    private int role;

    @NotNull(message = "phone must not be null")
    private String phoneNumber;

    private String city;

    private boolean status;

}
