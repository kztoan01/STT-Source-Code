package com.STT.UserService.Controller;


import com.STT.UserService.DTOs.LoginDTO;
import com.STT.UserService.DTOs.ResponseObject;
import com.STT.UserService.DTOs.UserDTO;
import com.STT.UserService.Service.UsersService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    @Autowired
    UsersService usersService;

    @PostMapping("/register")
    public ResponseObject register( @RequestBody UserDTO userDTO)
    {
       return usersService.register(userDTO);
    }

    @PostMapping("/login")
    public ResponseObject login( @RequestBody LoginDTO loginDTO)
    {
        return usersService.login(loginDTO);
    }

}
