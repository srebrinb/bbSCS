
package bobs.paraf.web;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonAnyGetter;
import com.fasterxml.jackson.annotation.JsonAnySetter;
import com.fasterxml.jackson.annotation.JsonIgnore;
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
    "extensions",
    "chain",
    "certX509",
    "valid",
    "version",
    "status",
    "reasonCode"
})
public class SelectCertResponse {

    /**
     * E=test@test.bg, CN=Test Profesionalen, OU=BULSTAT:1111111111, OU=Professional Certificate - UES, O=TEST AD, S=EGN:1111111111, C=BG
     * 
     */
    @JsonProperty("subject")
    private String Subject;
    /**
     *  009BA6E2
     * 
     */
    @JsonProperty("serialNumber")
    private String SerialNumber;
    /**
     * ....., C=BG
     * 
     */
    @JsonProperty("issuer")
    private String Issuer;
    /**
     *  3C0DB07FCEECCB9DD3D961D9EA3F560CA1EB2403
     * 
     */
    @JsonProperty("thumbprint")
    private String Thumbprint;
    /**
     *  2016-05-12T15:08:58
     * 
     */
    @JsonProperty("dateTimeNotAfter")
    private String DateTimeNotAfter;
    /**
     *  2016-05-12T15:08:58
     * 
     */
    @JsonProperty("dateTimeNotBefore")
    private String DateTimeNotBefore;
    @JsonProperty("extensions")
    private List<Extension> Extensions = new ArrayList<Extension>();
    @JsonProperty("chain")
    private List<String> chain = new ArrayList<String>();
    @JsonProperty("certX509")
    private String CertX509;
    @JsonProperty("valid")
    private Boolean Valid;
    @JsonProperty("version")
    private String version;
    @JsonProperty("status")
    private String status;
    @JsonProperty("reasonCode")
    private Integer reasonCode;
    @JsonIgnore
    private Map<String, Object> additionalProperties = new HashMap<String, Object>();

    /**
     * E=test@test.bg, CN=Test Profesionalen, OU=BULSTAT:1111111111, OU=Professional Certificate - UES, O=TEST AD, S=EGN:1111111111, C=BG
     * 
     * @return
     *     The Subject
     */
    @JsonProperty("subject")
    public String getSubject() {
        return Subject;
    }

    /**
     * E=test@test.bg, CN=Test Profesionalen, OU=BULSTAT:1111111111, OU=Professional Certificate - UES, O=TEST AD, S=EGN:1111111111, C=BG
     * 
     * @param Subject
     *     The Subject
     */
    @JsonProperty("subject")
    public void setSubject(String Subject) {
        this.Subject = Subject;
    }

    /**
     *  009BA6E2
     * 
     * @return
     *     The SerialNumber
     */
    @JsonProperty("serialNumber")
    public String getSerialNumber() {
        return SerialNumber;
    }

    /**
     *  009BA6E2
     * 
     * @param SerialNumber
     *     The SerialNumber
     */
    @JsonProperty("serialNumber")
    public void setSerialNumber(String SerialNumber) {
        this.SerialNumber = SerialNumber;
    }

    /**
     * ....., C=BG
     * 
     * @return
     *     The Issuer
     */
    @JsonProperty("issuer")
    public String getIssuer() {
        return Issuer;
    }

    /**
     * ....., C=BG
     * 
     * @param Issuer
     *     The Issuer
     */
    @JsonProperty("issuer")
    public void setIssuer(String Issuer) {
        this.Issuer = Issuer;
    }

    /**
     *  3C0DB07FCEECCB9DD3D961D9EA3F560CA1EB2403
     * 
     * @return
     *     The Thumbprint
     */
    @JsonProperty("thumbprint")
    public String getThumbprint() {
        return Thumbprint;
    }

    /**
     *  3C0DB07FCEECCB9DD3D961D9EA3F560CA1EB2403
     * 
     * @param Thumbprint
     *     The Thumbprint
     */
    @JsonProperty("thumbprint")
    public void setThumbprint(String Thumbprint) {
        this.Thumbprint = Thumbprint;
    }

    /**
     *  2016-05-12T15:08:58
     * 
     * @return
     *     The DateTimeNotAfter
     */
    @JsonProperty("dateTimeNotAfter")
    public String getDateTimeNotAfter() {
        return DateTimeNotAfter;
    }

    /**
     *  2016-05-12T15:08:58
     * 
     * @param DateTimeNotAfter
     *     The DateTimeNotAfter
     */
    @JsonProperty("dateTimeNotAfter")
    public void setDateTimeNotAfter(String DateTimeNotAfter) {
        this.DateTimeNotAfter = DateTimeNotAfter;
    }

    /**
     *  2016-05-12T15:08:58
     * 
     * @return
     *     The DateTimeNotBefore
     */
    @JsonProperty("dateTimeNotBefore")
    public String getDateTimeNotBefore() {
        return DateTimeNotBefore;
    }

    /**
     *  2016-05-12T15:08:58
     * 
     * @param DateTimeNotBefore
     *     The DateTimeNotBefore
     */
    @JsonProperty("dateTimeNotBefore")
    public void setDateTimeNotBefore(String DateTimeNotBefore) {
        this.DateTimeNotBefore = DateTimeNotBefore;
    }

    /**
     * 
     * @return
     *     The Extensions
     */
    @JsonProperty("dxtensions")
    public List<Extension> getExtensions() {
        return Extensions;
    }

    /**
     * 
     * @param Extensions
     *     The Extensions
     */
    @JsonProperty("extensions")
    public void setExtensions(List<Extension> Extensions) {
        this.Extensions = Extensions;
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

    /**
     * 
     * @return
     *     The CertX509
     */
    @JsonProperty("certX509")
    public String getCertX509() {
        return CertX509;
    }

    /**
     * 
     * @param CertX509
     *     The CertX509
     */
    @JsonProperty("certX509")
    public void setCertX509(String CertX509) {
        this.CertX509 = CertX509;
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

    /**
     * 
     * @return
     *     The version
     */
    @JsonProperty("version")
    public String getVersion() {
        return version;
    }

    /**
     * 
     * @param version
     *     The version
     */
    @JsonProperty("version")
    public void setVersion(String version) {
        this.version = version;
    }

    /**
     * 
     * @return
     *     The status
     */
    @JsonProperty("status")
    public String getStatus() {
        return status;
    }

    /**
     * 
     * @param status
     *     The status
     */
    @JsonProperty("status")
    public void setStatus(String status) {
        this.status = status;
    }

    /**
     * 
     * @return
     *     The reasonCode
     */
    @JsonProperty("reasonCode")
    public Integer getReasonCode() {
        return reasonCode;
    }

    /**
     * 
     * @param reasonCode
     *     The reasonCode
     */
    @JsonProperty("reasonCode")
    public void setReasonCode(Integer reasonCode) {
        this.reasonCode = reasonCode;
    }

    @JsonAnyGetter
    public Map<String, Object> getAdditionalProperties() {
        return this.additionalProperties;
    }

    @JsonAnySetter
    public void setAdditionalProperty(String name, Object value) {
        this.additionalProperties.put(name, value);
    }

}
