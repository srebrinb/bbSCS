
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
    "selector"
})
public class SelectCertRequest {

    @JsonProperty("version")
    private SelectCertRequest.Version version;
    @JsonProperty("selector")
    private Selector selector;

    /**
     * 
     * @return
     *     The version
     */
    @JsonProperty("version")
    public SelectCertRequest.Version getVersion() {
        return version;
    }

    /**
     * 
     * @param version
     *     The version
     */
    @JsonProperty("version")
    public void setVersion(SelectCertRequest.Version version) {
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

    @Generated("org.jsonschema2pojo")
    public static enum Version {

        _1_0("1.0");
        private final String value;
        private final static Map<String, SelectCertRequest.Version> CONSTANTS = new HashMap<String, SelectCertRequest.Version>();

        static {
            for (SelectCertRequest.Version c: values()) {
                CONSTANTS.put(c.value, c);
            }
        }

        private Version(String value) {
            this.value = value;
        }

        @JsonValue
        @Override
        public String toString() {
            return this.value;
        }

        @JsonCreator
        public static SelectCertRequest.Version fromValue(String value) {
            SelectCertRequest.Version constant = CONSTANTS.get(value);
            if (constant == null) {
                throw new IllegalArgumentException(value);
            } else {
                return constant;
            }
        }

    }

}
