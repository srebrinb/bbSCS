
package bobs.paraf.web;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonAnyGetter;
import com.fasterxml.jackson.annotation.JsonAnySetter;
import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import com.fasterxml.jackson.annotation.JsonValue;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "version",
    "selector",
    "content",
    "contents",
    "contentType",
    "hashAlgorithm",
    "signatureType",
    "forceSelectCert",
    "forcePINRquest",
    "protectedPin"
})
public class SignRequest {

    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("version")
    private String version = "1.0";
    @JsonProperty("selector")
    private Selector selector;
    /**
     * content Base64 encoding
     * 
     */
    @JsonProperty("content")
    private String content = "";
    /**
     * Array from content Base64 encoding
     * 
     */
    @JsonProperty("contents")
    private List<Object> contents = new ArrayList<Object>();
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("contentType")
    private SignRequest.ContentType contentType = SignRequest.ContentType.fromValue("data");
    @JsonProperty("hashAlgorithm")
    private SignRequest.HashAlgorithm hashAlgorithm = SignRequest.HashAlgorithm.fromValue("SHA1");
    /**
     * 
     * (Required)
     * 
     */
    @JsonProperty("signatureType")
    private SignRequest.SignatureType signatureType = SignRequest.SignatureType.fromValue("signature");
    /**
     * Force Select Cert
     * 
     */
    @JsonProperty("forceSelectCert")
    private Boolean forceSelectCert = false;
    /**
     * Force Prompt Enter PIN
     * 
     */
    @JsonProperty("forcePINRquest")
    private Boolean forcePINRquest = false;
    /**
     * protectedPinBase64 encoding
     * 
     */
    @JsonProperty("protectedPin")
    private String protectedPin = "";
    @JsonIgnore
    private Map<String, Object> additionalProperties = new HashMap<String, Object>();

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
     * @return
     *     The selector
     */
    @JsonProperty("selector")
    public Selector getSelector() {
        return selector;
    }

    /**
     * 
     * @param selector
     *     The selector
     */
    @JsonProperty("selector")
    public void setSelector(Selector selector) {
        this.selector = selector;
    }

    /**
     * content Base64 encoding
     * 
     * @return
     *     The content
     */
    @JsonProperty("content")
    public String getContent() {
        return content;
    }

    /**
     * content Base64 encoding
     * 
     * @param content
     *     The content
     */
    @JsonProperty("content")
    public void setContent(String content) {
        this.content = content;
    }

    /**
     * Array from content Base64 encoding
     * 
     * @return
     *     The contents
     */
    @JsonProperty("contents")
    public List<Object> getContents() {
        return contents;
    }

    /**
     * Array from content Base64 encoding
     * 
     * @param contents
     *     The contents
     */
    @JsonProperty("contents")
    public void setContents(List<Object> contents) {
        this.contents = contents;
    }

    /**
     * 
     * (Required)
     * 
     * @return
     *     The contentType
     */
    @JsonProperty("contentType")
    public SignRequest.ContentType getContentType() {
        return contentType;
    }

    /**
     * 
     * (Required)
     * 
     * @param contentType
     *     The contentType
     */
    @JsonProperty("contentType")
    public void setContentType(SignRequest.ContentType contentType) {
        this.contentType = contentType;
    }

    /**
     * 
     * @return
     *     The hashAlgorithm
     */
    @JsonProperty("hashAlgorithm")
    public SignRequest.HashAlgorithm getHashAlgorithm() {
        return hashAlgorithm;
    }

    /**
     * 
     * @param hashAlgorithm
     *     The hashAlgorithm
     */
    @JsonProperty("hashAlgorithm")
    public void setHashAlgorithm(SignRequest.HashAlgorithm hashAlgorithm) {
        this.hashAlgorithm = hashAlgorithm;
    }

    /**
     * 
     * (Required)
     * 
     * @return
     *     The signatureType
     */
    @JsonProperty("signatureType")
    public SignRequest.SignatureType getSignatureType() {
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
    public void setSignatureType(SignRequest.SignatureType signatureType) {
        this.signatureType = signatureType;
    }

    /**
     * Force Select Cert
     * 
     * @return
     *     The forceSelectCert
     */
    @JsonProperty("forceSelectCert")
    public Boolean getForceSelectCert() {
        return forceSelectCert;
    }

    /**
     * Force Select Cert
     * 
     * @param forceSelectCert
     *     The forceSelectCert
     */
    @JsonProperty("forceSelectCert")
    public void setForceSelectCert(Boolean forceSelectCert) {
        this.forceSelectCert = forceSelectCert;
    }

    /**
     * Force Prompt Enter PIN
     * 
     * @return
     *     The forcePINRquest
     */
    @JsonProperty("forcePINRquest")
    public Boolean getForcePINRquest() {
        return forcePINRquest;
    }

    /**
     * Force Prompt Enter PIN
     * 
     * @param forcePINRquest
     *     The forcePINRquest
     */
    @JsonProperty("forcePINRquest")
    public void setForcePINRquest(Boolean forcePINRquest) {
        this.forcePINRquest = forcePINRquest;
    }

    /**
     * protectedPinBase64 encoding
     * 
     * @return
     *     The protectedPin
     */
    @JsonProperty("protectedPin")
    public String getProtectedPin() {
        return protectedPin;
    }

    /**
     * protectedPinBase64 encoding
     * 
     * @param protectedPin
     *     The protectedPin
     */
    @JsonProperty("protectedPin")
    public void setProtectedPin(String protectedPin) {
        this.protectedPin = protectedPin;
    }

    @JsonAnyGetter
    public Map<String, Object> getAdditionalProperties() {
        return this.additionalProperties;
    }

    @JsonAnySetter
    public void setAdditionalProperty(String name, Object value) {
        this.additionalProperties.put(name, value);
    }

    @Generated("org.jsonschema2pojo")
    public static enum ContentType {

        DATA("data"),
        DIGEST("digest");
        private final String value;
        private final static Map<String, SignRequest.ContentType> CONSTANTS = new HashMap<String, SignRequest.ContentType>();

        static {
            for (SignRequest.ContentType c: values()) {
                CONSTANTS.put(c.value, c);
            }
        }

        private ContentType(String value) {
            this.value = value;
        }

        @JsonValue
        @Override
        public String toString() {
            return this.value;
        }

        @JsonCreator
        public static SignRequest.ContentType fromValue(String value) {
            SignRequest.ContentType constant = CONSTANTS.get(value);
            if (constant == null) {
                throw new IllegalArgumentException(value);
            } else {
                return constant;
            }
        }

    }

    @Generated("org.jsonschema2pojo")
    public static enum HashAlgorithm {

        SHA_1("SHA1"),
        SHA_256("SHA256"),
        SHA_384("SHA384"),
        SHA_512("SHA512");
        private final String value;
        private final static Map<String, SignRequest.HashAlgorithm> CONSTANTS = new HashMap<String, SignRequest.HashAlgorithm>();

        static {
            for (SignRequest.HashAlgorithm c: values()) {
                CONSTANTS.put(c.value, c);
            }
        }

        private HashAlgorithm(String value) {
            this.value = value;
        }

        @JsonValue
        @Override
        public String toString() {
            return this.value;
        }

        @JsonCreator
        public static SignRequest.HashAlgorithm fromValue(String value) {
            SignRequest.HashAlgorithm constant = CONSTANTS.get(value);
            if (constant == null) {
                throw new IllegalArgumentException(value);
            } else {
                return constant;
            }
        }

    }

    @Generated("org.jsonschema2pojo")
    public static enum SignatureType {

        SIGNATURE("signature");
        private final String value;
        private final static Map<String, SignRequest.SignatureType> CONSTANTS = new HashMap<String, SignRequest.SignatureType>();

        static {
            for (SignRequest.SignatureType c: values()) {
                CONSTANTS.put(c.value, c);
            }
        }

        private SignatureType(String value) {
            this.value = value;
        }

        @JsonValue
        @Override
        public String toString() {
            return this.value;
        }

        @JsonCreator
        public static SignRequest.SignatureType fromValue(String value) {
            SignRequest.SignatureType constant = CONSTANTS.get(value);
            if (constant == null) {
                throw new IllegalArgumentException(value);
            } else {
                return constant;
            }
        }

    }

}
