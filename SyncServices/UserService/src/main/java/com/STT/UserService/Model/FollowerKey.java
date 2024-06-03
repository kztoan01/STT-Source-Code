package com.STT.UserService.Model;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;


@Embeddable
@AllArgsConstructor
@NoArgsConstructor
@Data
public class FollowerKey implements Serializable {

    @Column(name = "userId")
    private Integer userId;


    @Column(name = "artistId")
    private Integer artistId;


}
