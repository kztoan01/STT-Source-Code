package com.STT.UserService.Model;


import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.Date;

@Entity
@Table(name = "Followers")
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Followers {

    @Id
    @EmbeddedId
    FollowerKey  id;

    @ManyToOne
    @MapsId("userId")
    @JoinColumn(name = "userId")
    private Users users;

    @ManyToOne
    @MapsId("artistId")
    @JoinColumn(name = "artistId")
    private Artists artists;

    @Column(name = "beginDate",nullable = false, length = 100)
    private Date beginDate;


}
