package com.STT.UserService.Model;


import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.UuidGenerator;

import java.util.*;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "Users")
public class Users {


      @Id
      @GeneratedValue(strategy = GenerationType.IDENTITY)
      @Column(name = "id",nullable = false, length = 100)
      private Integer id;


      @UuidGenerator(style = UuidGenerator.Style.TIME)
      @Column(name = "userId",nullable = false, length = 100)
      private UUID userId;


      @Column(name = "country", length = 100)
      private String country;


      @Column(name = "username",nullable = false, length = 100)
      private String username;

      @Column(name = "password",nullable = false, length = 100)
      private  String password;

      @Column(name = "fullName", length = 100)
      private String fullName;

      @Column(name = "birthDate", length = 100)
      private Date birthDate;

      @Column(name = "phoneNumber",nullable = false, length = 100)
      private String phoneNumber;

      @Column(name = "emailAddress",nullable = false, length = 100)
      private  String emailAddress;

      @Column(name = "city", length = 100)
      private String city;

      @Column(name = "status",nullable = false, length = 100)
      private Boolean status;

      @OneToMany(mappedBy = "users")
      private List<Followers> followers;

      @OneToOne(mappedBy = "users")
      private Artists artists;

      @Column(name = "role",nullable = false, length = 100)
      private Integer role;

      public Collection<UserRole> getAuthorities() {
            List<UserRole> authorities = new ArrayList<>();

            switch (role) {
                  case 1:
                        authorities.add(UserRole.USER);
                        break;
                  case 2:
                        authorities.add(UserRole.MANAGER);
                        break;
                  case 3:
                        authorities.add(UserRole.ADMIN);
                        break;
                  default:
                        authorities.add(UserRole.USER);
            }

            return authorities;
      }

      public Integer getId() {
            return id;
      }

      public void setId(Integer id) {
            this.id = id;
      }

      public UUID getUserId() {
            return userId;
      }

      public void setUserId(UUID userId) {
            this.userId = userId;
      }

      public String getCountry() {
            return country;
      }

      public void setCountry(String country) {
            this.country = country;
      }

      public String getUsername() {
            return username;
      }

      public void setUsername(String username) {
            this.username = username;
      }

      public String getPassword() {
            return password;
      }

      public void setPassword(String password) {
            this.password = password;
      }

      public String getFullName() {
            return fullName;
      }

      public void setFullName(String fullName) {
            this.fullName = fullName;
      }

      public Date getBirthDate() {
            return birthDate;
      }

      public void setBirthDate(Date birthDate) {
            this.birthDate = birthDate;
      }

      public String getPhoneNumber() {
            return phoneNumber;
      }

      public void setPhoneNumber(String phoneNumber) {
            this.phoneNumber = phoneNumber;
      }

      public String getEmailAddress() {
            return emailAddress;
      }

      public void setEmailAddress(String emailAddress) {
            this.emailAddress = emailAddress;
      }

      public String getCity() {
            return city;
      }

      public void setCity(String city) {
            this.city = city;
      }

      public Boolean getStatus() {
            return status;
      }

      public void setStatus(Boolean status) {
            this.status = status;
      }

      public List<Followers> getFollowers() {
            return followers;
      }

      public void setFollowers(List<Followers> followers) {
            this.followers = followers;
      }

      public Artists getArtists() {
            return artists;
      }

      public void setArtists(Artists artists) {
            this.artists = artists;
      }

      public Integer getRole() {
            return role;
      }

      public void setRole(Integer role) {
            this.role = role;
      }
}
