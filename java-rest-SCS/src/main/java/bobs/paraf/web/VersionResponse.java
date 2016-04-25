/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package bobs.paraf.web;


import java.util.ArrayList;
import java.util.List;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonInclude.Include;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;

/*
{
"version": "1.0",
"httpMethods": "GET, POST",
"contentTypes": "data, digest",
"signatureTypes": "signature",
"selectorAvailable": true,
"hashAlgorithms": "SHA1, SHA256, SHA384, SHA512"
}
*/
@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
public class VersionResponse {
    @JsonProperty("version")
    public String version = "1.0";
    @JsonProperty("httpMethods")
    public String httpMethods = "POST";
    @JsonProperty("contentTypes")
    public String contentTypes = "data";
    @JsonProperty("selectorAvailable")
    public boolean  selectorAvailable = true;
    @JsonProperty("hashAlgorithms")
    public String  hashAlgorithms = "SHA1, SHA256, SHA384, SHA512";
}
