package com.STT.UserService.Service;
import com.STT.UserService.Config.JWTUtils;
import com.STT.UserService.DTOs.ErrorDTO;
import com.STT.UserService.DTOs.LoginDTO;
import com.STT.UserService.DTOs.ResponseObject;
import com.STT.UserService.DTOs.UserDTO;
import com.STT.UserService.Model.Users;
import com.STT.UserService.Repository.UsersRepository;
import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Objects;
import java.util.regex.Pattern;

@Service
public class UsersService implements  IUsersService {


    @Autowired
    JWTUtils jwtUtils;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Autowired
    UsersRepository usersRepository;

    public static boolean patternMatches(String emailAddress, String regexPattern) {
        return Pattern.compile(regexPattern)
                .matcher(emailAddress)
                .matches();
    }
    @Override
    public ResponseObject register(UserDTO userDTO) {
        ResponseObject responseObject = new ResponseObject();
        if(userDTO != null)
        {
            boolean isExisted = false;
            List<ErrorDTO> errors= new ArrayList<>();
            if(usersRepository.existUserByPhoneNumber(userDTO.getPhoneNumber())) {
                errors.add(new ErrorDTO("DuplicatedPhonenumber","Phone number is registered!"));
                isExisted=true;
            }
            if(usersRepository.existsUserByUserName(userDTO.getUsername())) {
                errors.add(new ErrorDTO("DuplicatedUsername","User name is taken!"));
                isExisted=true;
            }
            if(usersRepository.existUserByEmailAddress(userDTO.getEmailAddress())) {
                errors.add(new ErrorDTO("DuplicatedGmail","Email address is taken!"));
                isExisted=true;
            }
            if(userDTO.getPhoneNumber().length() != 10)
            {
                errors.add(new ErrorDTO("PhonenumberFormat","Phone number is invalid!"));
                isExisted=true;
            }
            if(!patternMatches(userDTO.getEmailAddress(), "^[a-zA-Z0-9_!#$%&'*+/=?`{|}~^.-]+@[a-zA-Z0-9.-]+$"))
            {
                errors.add(new ErrorDTO("GmailFormat","Gmail address is invalid!"));
                isExisted=true;
            }
            if(isExisted)
            {
                responseObject.setChangeSuccessfully(false);
                responseObject.setErrors(errors);
                responseObject.setStatusCode(HttpStatus.NOT_ACCEPTABLE.value());
            }
            else
            {
                Users user = new Users();
                BeanUtils.copyProperties(userDTO,user);
                user.setPassword(passwordEncoder.encode(user.getPassword()));
                user.setStatus(true);
                Users savedUser = usersRepository.save(user);
                UserDTO returnUserDT0 = new UserDTO();
                BeanUtils.copyProperties(savedUser,returnUserDT0);
                responseObject.setChangeSuccessfully(true);
                responseObject.setData(returnUserDT0);
            }

        }
        else
        {
            responseObject.setMessage("User is null!");
        }
        return responseObject;
    }

    @Override
    public ResponseObject login(LoginDTO loginDTO) {
        ResponseObject response = new ResponseObject();
        try {
            Users user = usersRepository.findByEmailAddress(loginDTO.getEmail()).orElse(null);
            if(user != null && passwordEncoder.matches(loginDTO.getPassword(), user.getPassword()))  {
                var jwt = jwtUtils.generateToken(user);
                var refreshToken = jwtUtils.generateRefreshToken(new HashMap<>(), user);
                response.setStatusCode(200);
                response.setToken(jwt);
                response.setRefreshToken(refreshToken);
                response.setExpirationTime("24Hr");
                response.setMessage("Successfully Signed In");
                UserDTO userDTO  = new UserDTO();
                BeanUtils.copyProperties(user,userDTO);
                response.setData(userDTO);
            } else {
                response.setStatusCode(404);
                response.getErrors().add(new ErrorDTO("LoginFalse","Username or password is incorrect!"));
            }
        } catch (Exception e) {
            response.setStatusCode(500);
            response.getErrors().add(new ErrorDTO("Login error",e.getMessage()));
        }
        return response;
    }
}
