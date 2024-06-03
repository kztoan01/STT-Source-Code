package com.STT.UserService.Model;


import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.UuidGenerator;

import java.util.Date;
import java.util.List;

@Entity
@Table(name = "Artists")
@AllArgsConstructor
@NoArgsConstructor
@Data
public class Artists {

        @Id
        @GeneratedValue(strategy = GenerationType.IDENTITY)
        @Column(name = "id",nullable = false)
        private Integer id;

        @UuidGenerator(style = UuidGenerator.Style.TIME)
        @Column(name = "artistId",length = 100,nullable = false)
        private String artistId;


        @Column(name = "fullName",length = 100,nullable = false)
        private String fullName;

        @Column(name = "profile",length = 100)
        private String profile;

        @Column(name = "birthDate")
        private Date birthDate;

        @OneToOne
        private Users users;

        @OneToMany(mappedBy = "artists")
        private List<Followers> followers;

}
