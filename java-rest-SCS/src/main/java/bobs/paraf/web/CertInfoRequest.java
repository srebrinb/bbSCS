
package bobs.paraf.web;

import java.util.HashMap;
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
    "version",
    "certX509",
    "profiles"
})
public class CertInfoRequest {

    @JsonProperty("version")
    private String version;
    @JsonProperty("certX509")
    private String certX509;
    @JsonProperty("profiles")
    private CertInfoRequest.Profiles profiles;

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
     *     The certX509
     */
    @JsonProperty("certX509")
    public String getCertX509() {
        return certX509;
    }

    /**
     * 
     * @param certX509
     *     The certX509
     */
    @JsonProperty("certX509")
    public void setCertX509(String certX509) {
        this.certX509 = certX509;
    }

    /**
     * 
     * @return
     *     The profiles
     */
    @JsonProperty("profiles")
    public CertInfoRequest.Profiles getProfiles() {
        return profiles;
    }

    /**
     * 
     * @param profiles
     *     The profiles
     */
    @JsonProperty("profiles")
    public void setProfiles(CertInfoRequest.Profiles profiles) {
        this.profiles = profiles;
    }

    @Generated("org.jsonschema2pojo")
    public static enum Profiles {

        NONE("none"),
        BASE("base"),
        EXTENSIONS("extensions"),
        CHAIN("chain"),
        CERTX_509("certx509");
        private final String value;
        private final static Map<String, CertInfoRequest.Profiles> CONSTANTS = new HashMap<String, CertInfoRequest.Profiles>();

        static {
            for (CertInfoRequest.Profiles c: values()) {
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
        public static CertInfoRequest.Profiles fromValue(String value) {
            CertInfoRequest.Profiles constant = CONSTANTS.get(value);
            if (constant == null) {
                throw new IllegalArgumentException(value);
            } else {
                return constant;
            }
        }

    }

}
