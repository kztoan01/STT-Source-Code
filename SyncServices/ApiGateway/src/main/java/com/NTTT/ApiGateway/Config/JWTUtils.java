package com.NTTT.ApiGateway.Config;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import org.springframework.stereotype.Component;

import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.function.Function;

@Component
public class JWTUtils {

    private SecretKey Key;
    private  static  final long EXPIRATION_TIME = 86400000*7; //24hours or 86400000 milisecs
    public JWTUtils(){
        String secreteString = "843567893696976453275974432697R634976R738467TR678T34865R6834R8763T478378637664538745673865783678548735687R3";
        byte[] keyBytes = Base64.getDecoder().decode(secreteString.getBytes(StandardCharsets.UTF_8));
        this.Key = new SecretKeySpec(keyBytes, "HmacSHA256");
    }


    public String extractEmail(String token){
        return extractClaims(token).getSubject();
    }

    public List<String> extractUserRoles(String token) {
        Claims claims = extractClaims(token);
        List<String> roles = new ArrayList<>();
        if (claims != null) {
            Object rolesObject = claims.get("UserRole");
            if (rolesObject instanceof List) {
                for (Object role : (List<?>) rolesObject) {
                    if (role instanceof String) {
                        roles.add((String) role);
                    }
                }
            }
        }
        return roles;
    }


    private Claims extractClaims(String token) {
        Claims claims;
        try {
            claims = Jwts.parser()
                    .setSigningKey("843567893696976453275974432697R634976R738467TR678T34865R6834R8763T478378637664538745673865783678548735687R3")
                    .parseClaimsJws(token)
                    .getBody();
        } catch (Exception e) {
            claims = null;
        }
        return claims;
    };
    public boolean isTokenValid(String token, String APIuserName){
        final String username = extractEmail(token);
        return (username.equals(APIuserName) && !isTokenExpired(token));
    }
    public boolean isTokenExpired(String token){
        return extractClaims(token).getExpiration().before(new Date());
    }

}
