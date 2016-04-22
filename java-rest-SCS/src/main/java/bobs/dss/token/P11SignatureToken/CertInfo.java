
package bobs.dss.token.P11SignatureToken;

import java.util.ArrayList;
import java.util.List;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "subject",
    "serialNumber",
    "issuer",
    "thumbprint",
    "dateTimeNotAfter",
    "dateTimeNotBefore",
    "chain",
    "valid",
    "tokenInfo"
})
public class CertInfo {

    @JsonProperty("subject")
    private String Subject;
    @JsonProperty("serialNumber")
    private String SerialNumber;
    @JsonProperty("issuer")
    private String Issuer;
    @JsonProperty("thumbprint")
    private String Thumbprint;
    @JsonProperty("dateTimeNotAfter")
    private String DateTimeNotAfter;
    @JsonProperty("dateTimeNotBefore")
    private String DateTimeNotBefore;
    @JsonProperty("chain")
    private List<String> chain = new ArrayList<String>();
    @JsonProperty("valid")
    private Boolean Valid;
    @JsonProperty("tokenInfo")
    private TokenInfo tokenInfo;
    @JsonProperty("PKCS11modulePath")
    private String PKCS11modulePath;

    /**
     * 
     * @return
     *     The Subject
     */
    @JsonProperty("subject")
    public String getSubject() {
        return Subject;
    }

    /**
     * 
     * @param Subject
     *     The Subject
     */
    @JsonProperty("subject")
    public void setSubject(String Subject) {
        this.Subject = Subject;
    }

    public CertInfo withSubject(String Subject) {
        this.Subject = Subject;
        return this;
    }

    /**
     * 
     * @return
     *     The SerialNumber
     */
    @JsonProperty("serialNumber")
    public String getSerialNumber() {
        return SerialNumber;
    }

    /**
     * 
     * @param SerialNumber
     *     The SerialNumber
     */
    @JsonProperty("serialNumber")
    public void setSerialNumber(String SerialNumber) {
        this.SerialNumber = SerialNumber;
    }

    public CertInfo withSerialNumber(String SerialNumber) {
        this.SerialNumber = SerialNumber;
        return this;
    }

    /**
     * 
     * @return
     *     The Issuer
     */
    @JsonProperty("issuer")
    public String getIssuer() {
        return Issuer;
    }

    /**
     * 
     * @param Issuer
     *     The Issuer
     */
    @JsonProperty("issuer")
    public void setIssuer(String Issuer) {
        this.Issuer = Issuer;
    }

    public CertInfo withIssuer(String Issuer) {
        this.Issuer = Issuer;
        return this;
    }
//PKCS11modulePath
    /**
     * 
     * @return
     *     The Issuer
     */
    @JsonProperty("PKCS11modulePath")
    public String getPKCS11modulePath() {
        return PKCS11modulePath;
    }

    /**
     * 
     * @param Issuer
     *     The Issuer
     */
    @JsonProperty("PKCS11modulePath")
    public void setPKCS11modulePath(String PKCS11modulePath) {
        this.PKCS11modulePath = PKCS11modulePath;
    }

    public CertInfo withPKCS11modulePath(String PKCS11modulePath) {
        this.Issuer = Issuer;
        return this;
    }
    /**
     * 
     * @return
     *     The Thumbprint
     */
    @JsonProperty("thumbprint")
    public String getThumbprint() {
        return Thumbprint;
    }

    /**
     * 
     * @param Thumbprint
     *     The Thumbprint
     */
    @JsonProperty("thumbprint")
    public void setThumbprint(String Thumbprint) {
        this.Thumbprint = Thumbprint;
    }

    public CertInfo withThumbprint(String Thumbprint) {
        this.Thumbprint = Thumbprint;
        return this;
    }

    /**
     * 
     * @return
     *     The DateTimeNotAfter
     */
    @JsonProperty("dateTimeNotAfter")
    public String getDateTimeNotAfter() {
        return DateTimeNotAfter;
    }

    /**
     * 
     * @param DateTimeNotAfter
     *     The DateTimeNotAfter
     */
    @JsonProperty("dateTimeNotAfter")
    public void setDateTimeNotAfter(String DateTimeNotAfter) {
        this.DateTimeNotAfter = DateTimeNotAfter;
    }

    public CertInfo withDateTimeNotAfter(String DateTimeNotAfter) {
        this.DateTimeNotAfter = DateTimeNotAfter;
        return this;
    }

    /**
     * 
     * @return
     *     The DateTimeNotBefore
     */
    @JsonProperty("dateTimeNotBefore")
    public String getDateTimeNotBefore() {
        return DateTimeNotBefore;
    }

    /**
     * 
     * @param DateTimeNotBefore
     *     The DateTimeNotBefore
     */
    @JsonProperty("dateTimeNotBefore")
    public void setDateTimeNotBefore(String DateTimeNotBefore) {
        this.DateTimeNotBefore = DateTimeNotBefore;
    }

    public CertInfo withDateTimeNotBefore(String DateTimeNotBefore) {
        this.DateTimeNotBefore = DateTimeNotBefore;
        return this;
    }

    /**
     * 
     * @return
     *     The chain
     */
    @JsonProperty("chain")
    public List<String> getChain() {
        return chain;
    }

    /**
     * 
     * @param chain
     *     The chain
     */
    @JsonProperty("chain")
    public void setChain(List<String> chain) {
        this.chain = chain;
    }

    public CertInfo withChain(List<String> chain) {
        this.chain = chain;
        return this;
    }

    /**
     * 
     * @return
     *     The Valid
     */
    @JsonProperty("valid")
    public Boolean getValid() {
        return Valid;
    }

    /**
     * 
     * @param Valid
     *     The Valid
     */
    @JsonProperty("valid")
    public void setValid(Boolean Valid) {
        this.Valid = Valid;
    }

    public CertInfo withValid(Boolean Valid) {
        this.Valid = Valid;
        return this;
    }

    /**
     * 
     * @return
     *     The tokenInfo
     */
    @JsonProperty("tokenInfo")
    public TokenInfo getTokenInfo() {
        return tokenInfo;
    }

    /**
     * 
     * @param tokenInfo
     *     The tokenInfo
     */
    @JsonProperty("tokenInfo")
    public void setTokenInfo(TokenInfo tokenInfo) {
        this.tokenInfo = tokenInfo;
    }

    public CertInfo withTokenInfo(TokenInfo tokenInfo) {
        this.tokenInfo = tokenInfo;
        return this;
    }

}
