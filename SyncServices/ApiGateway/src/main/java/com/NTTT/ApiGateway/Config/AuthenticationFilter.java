package com.NTTT.ApiGateway.Config;

import com.NTTT.ApiGateway.Clients.ResponseObject;
import com.NTTT.ApiGateway.Clients.ResponseUserDTO;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.gateway.filter.GatewayFilter;
import org.springframework.cloud.gateway.filter.factory.AbstractGatewayFilterFactory;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Component;
import org.springframework.web.client.RestTemplate;

import java.util.List;

@Component
public class AuthenticationFilter extends AbstractGatewayFilterFactory<AuthenticationFilter.Config> {

    @Autowired
    private RouteValidator validator;

    @Autowired
    private RestTemplate template;

    @Autowired
    private JWTUtils jwtUtil ;

    public AuthenticationFilter() {
        super(Config.class);
    }

    Logger logger
            = LoggerFactory.getLogger(AuthenticationFilter.class);

    @Override
    public GatewayFilter apply(Config config) {
        return ((exchange, chain) -> {
            if (validator.isSecured.test(exchange.getRequest())) {
                // Header contains token or not
                if (!exchange.getRequest().getHeaders().containsKey(HttpHeaders.AUTHORIZATION)) {
                    exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                    return exchange.getResponse().setComplete();
                }

                String authHeader = exchange.getRequest().getHeaders().get(HttpHeaders.AUTHORIZATION).get(0);
                if (authHeader != null && authHeader.startsWith("Bearer ")) {
                    authHeader = authHeader.substring(7);
                }
                List<String> userRoles = jwtUtil.extractUserRoles(authHeader);

                try {
                    String email = jwtUtil.extractEmail(authHeader);
                    ResponseObject responseObject =  template.getForObject("http://localhost:8080/api/users/getByEmail/" + email, ResponseObject.class);

                    //ResponseObject responseObject =  template.getForObject("http://user-service:8080/users/getByEmail/" + email, ResponseObject.class);
                    if(responseObject.getStatusCode() == 200)
                    {
                        logger.info("Test:12");
                        String requestPath = exchange.getRequest().getPath().toString();
                        if (userRoles.contains("ADMIN")) {
                            logger.info("test Admin");
                            if (requestPath.contains("/users"))
                            {
                                return chain.filter(exchange);
                            }
                            else
                            {
                                exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                                return exchange.getResponse().setComplete();
                            }
                        }
                        else if (userRoles.contains("MANAGER")) {
                            logger.info("test Manager");
                            logger.info(requestPath);
                            if (requestPath.contains("/users"))
                            {
                                return chain.filter(exchange);
                            }
                            else
                            {
                                exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                                return exchange.getResponse().setComplete();
                            }
                        }
                        else if (userRoles.contains("USER")) {
                            logger.info("test User");
                             logger.info(requestPath);
                            if (requestPath.contains("/auth/")) {
                                return chain.filter(exchange);
                            } else {
                                exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                                return exchange.getResponse().setComplete();
                            }
                        }
                        else {
                            exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                            return exchange.getResponse().setComplete();
                        }
                    }
                    else
                    {
                        logger.info("Invalid access...![User not found]");
                        exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                        return exchange.getResponse().setComplete();
                    }
                } catch (Exception e) {
                    logger.info("Invalid access...![Error while getting user info]");
                    exchange.getResponse().setStatusCode(HttpStatus.FORBIDDEN);
                    return exchange.getResponse().setComplete();
                }
            }
            return chain.filter(exchange);
        });
    }

    public static class Config {

    }
}
