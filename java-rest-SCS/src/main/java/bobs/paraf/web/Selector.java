package bobs.paraf.web;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import com.fasterxml.jackson.annotation.JsonValue;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "issuers",
    "keyusages",
    "validate",
    "thumbprint",
    "profiles"
})
public class Selector {

    @JsonProperty("issuers")
    private List<String> issuers = new ArrayList<String>();
    @JsonProperty("keyusages")
    private List<Keyusage> keyusages = new ArrayList<Keyusage>();
    @JsonProperty("validate")
    private Boolean validate;
    @JsonProperty("thumbprint")
    private String thumbprint;
    @JsonProperty("serialNumber")
    private String serialNumber;
    @JsonProperty("profiles")
    private Selector.Profiles profiles;
    

    /**
     *
     * @return The issuers
     */
    @JsonProperty("issuers")
    public List<String> getIssuers() {
        return issuers;
    }

    /**
     *
     * @param issuers The issuers
     */
    @JsonProperty("issuers")
    public void setIssuers(List<String> issuers) {
        this.issuers = issuers;
    }

    /**
     *
     * @return The keyusages
     */
    @JsonProperty("keyusages")
    public List<Keyusage> getKeyusages() {
        return keyusages;
    }

    /**
     *
     * @param keyusages The keyusages
     */
    @JsonProperty("keyusages")
    public void setKeyusages(List<Keyusage> keyusages) {
        this.keyusages = keyusages;
    }

    /**
     *
     * @return The validate
     */
    @JsonProperty("validate")
    public Boolean getValidate() {
        return validate;
    }

    /**
     *
     * @param validate The validate
     */
    @JsonProperty("validate")
    public void setValidate(Boolean validate) {
        this.validate = validate;
    }

    /**
     *
     * @return The thumbprint
     */
    @JsonProperty("thumbprint")
    public String getThumbprint() {
        return thumbprint;
    }

    /**
     *
     * @param thumbprint The thumbprint
     */
    @JsonProperty("thumbprint")
    public void setThumbprint(String thumbprint) {
        this.thumbprint = thumbprint;
    }

    /**
     *
     * @return The profiles
     */
    @JsonProperty("profiles")
    public Selector.Profiles getProfiles() {
        return profiles;
    }

    /**
     *
     * @param profiles The profiles
     */
    @JsonProperty("profiles")
    public void setProfiles(Selector.Profiles profiles) {
        this.profiles = profiles;
    }

    /**
     * @return the serialNumber
     */
    
    @JsonProperty("serialNumber")
    public String getSerialNumber() {
        return serialNumber;
    }

    /**
     * @param serialNumber the serialNumber to set
     */
    @JsonProperty("serialNumber")
    public void setSerialNumber(String serialNumber) {
        this.serialNumber = serialNumber;
    }

    @Generated("org.jsonschema2pojo")
    public static enum Profiles {

        BASE("base"),
        EXTENSIONS("extensions"),
        CHAIN("chain"),
        CERTX_509("certx509");
        private final String value;
        private final static Map<String, Selector.Profiles> CONSTANTS = new HashMap<String, Selector.Profiles>();

        static {
            for (Selector.Profiles c : values()) {
                CONSTANTS.put(c.value, c);
            }
        }

        private Profiles(String value) {
            this.value = value;
        }

        @JsonValue
        @Override
        public String toString() {
            return this.value;
        }

        @JsonCreator
        public static Selector.Profiles fromValue(String value) {
            Selector.Profiles constant = CONSTANTS.get(value);
            if (constant == null) {
                throw new IllegalArgumentException(value);
            } else {
                return constant;
            }
        }

    }

}
