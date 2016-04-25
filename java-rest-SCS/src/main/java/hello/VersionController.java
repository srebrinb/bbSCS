/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package hello;

import bobs.paraf.web.SignResponse;
import bobs.paraf.web.VersionResponse;
import com.fasterxml.jackson.annotation.JsonProperty;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController

@CrossOrigin(origins = "*", maxAge = 3600)
public class VersionController {

    @RequestMapping("/version")
    public VersionResponse version() {
        return new VersionResponse();
    }
}
