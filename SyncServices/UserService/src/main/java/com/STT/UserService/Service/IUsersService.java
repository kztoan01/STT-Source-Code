package com.STT.UserService.Service;

import com.STT.UserService.DTOs.LoginDTO;
import com.STT.UserService.DTOs.ResponseObject;
import com.STT.UserService.DTOs.UserDTO;

public interface IUsersService {

    public ResponseObject register(UserDTO userDTO);

    public ResponseObject login (LoginDTO loginDTO);
}
