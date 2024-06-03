package com.STT.UserService.Repository;

import com.STT.UserService.DTOs.LoginDTO;
import com.STT.UserService.Model.Users;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.Optional;
import java.util.UUID;

@Repository
public interface UsersRepository extends JpaRepository<Users,Integer> {


    Optional<Users> findByEmailAddress(String emailAddress);
    Optional<Users> findByUserId(UUID userId);

    @Query("select count(p) = 1 from Users p where username = ?1")
    boolean existsUserByUserName(String username);

    @Query("select count(p) = 1 from Users p where phoneNumber = ?1")
    boolean existUserByPhoneNumber(String phonenumber);

    @Query("select count(p) = 1 from Users p where emailAddress = ?1")
    boolean existUserByEmailAddress(String emailAddress);


}
