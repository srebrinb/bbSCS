
package bobs.paraf.web;

import java.util.ArrayList;
import java.util.List;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonInclude.Include;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import eu.europa.esig.dss.x509.CertificateToken;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "version",
    "signatureAlgorithm",
    "signatureType",
    "signature",
    "signatures",
    "chain",
    "status",
    "reasonCode",
    "reasonText"
})
public class SignResponse {

    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("version")
    private String version = "1.0";
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("signatureAlgorithm")
    private String signatureAlgorithm;
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("signatureType")
    private String signatureType;
    @JsonProperty("signature")
    private String signature;
    @JsonProperty("signatures")
     @JsonInclude(Include.NON_EMPTY)
    private List<String> signatures = new ArrayList<String>();
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("chain")
     @JsonInclude(Include.NON_EMPTY)
    private List<String> chain = new ArrayList<String>();
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("status")
    private String status;
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("reasonCode")
    private Integer reasonCode;
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("reasonText")
    private String reasonText;

    /**
     * 
     * (Required)
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
     * (Required)
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
     * (Required)
     * 
     * @return
     *     The signatureAlgorithm
     */
    @JsonProperty("signatureAlgorithm")
    public String getSignatureAlgorithm() {
        return signatureAlgorithm;
    }

    /**
     * 
     * (Required)
     * 
     * @param signatureAlgorithm
     *     The signatureAlgorithm
     */
    @JsonProperty("signatureAlgorithm")
    public void setSignatureAlgorithm(String signatureAlgorithm) {
        this.signatureAlgorithm = signatureAlgorithm;
    }

    /**
     * 
     * (Required)
     * 
     * @return
     *     The signatureType
     */
    @JsonProperty("signatureType")
    public String getSignatureType() {
        return signatureType;
    }

    /**
     * 
     * (Required)
     * 
     * @param signatureType
     *     The signatureType
     */
    @JsonProperty("signatureType")
    public void setSignatureType(String signatureType) {
        this.signatureType = signatureType;
    }

    /**
     * 
     * @return
     *     The signature
     */
    @JsonProperty("signature")
    public String getSignature() {
        return signature;
    }

    /**
     * 
     * @param signature
     *     The signature
     */
    @JsonProperty("signature")
    public void setSignature(String signature) {
        this.signature = signature;
    }

    /**
     * 
     * @return
     *     The signatures
     */
    @JsonProperty("signatures")
    public List<String> getSignatures() {
        return signatures;
    }

    /**
     * 
     * @param signatures
     *     The signatures
     */
    @JsonProperty("signatures")
    public void setSignatures(List<String> signatures) {
        this.signatures = signatures;
    }

    /**
     * 
     * (Required)
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
     * (Required)
     * 
     * @param chain
     *     The chain
     */
    @JsonProperty("chain")
    public void setChain(List<String> chain) {
        this.chain = chain;
    }

    @JsonProperty("chain")
    public void setChain(CertificateToken[] certsToken) {
        this.chain.clear();
        for (CertificateToken certToken:certsToken){
            this.chain.add(certToken.getBase64Encoded());
        }
           
        
    }
    /**
     * 
     * (Required)
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
     * (Required)
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
     * (Required)
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
     * (Required)
     * 
     * @param reasonCode
     *     The reasonCode
     */
    @JsonProperty("reasonCode")
    public void setReasonCode(Integer reasonCode) {
        this.reasonCode = reasonCode;
    }

    /**
     * 
     * (Required)
     * 
     * @return
     *     The reasonText
     */
    @JsonProperty("reasonText")
    public String getReasonText() {
        return reasonText;
    }

    /**
     * 
     * (Required)
     * 
     * @param reasonText
     *     The reasonText
     */
    @JsonProperty("reasonText")
    public void setReasonText(String reasonText) {
        this.reasonText = reasonText;
    }

}
