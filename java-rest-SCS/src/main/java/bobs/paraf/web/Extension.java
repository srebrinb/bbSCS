
package bobs.paraf.web;

import java.util.HashMap;
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
    "oid",
    "name",
    "value",
    "rawValue"
})
public class Extension {

    /**
     *  2.5.29.14
     * 
     */
    @JsonProperty("oid")
    private String Oid;
    /**
     * Subject Key Identifier
     * 
     */
    @JsonProperty("name")
    private String Name;
    /**
     *  5E59E373F8A09CA780FCAA1BCE98E28C0A544376
     * 
     */
    @JsonProperty("value")
    private String Value;
    /**
     * BBReWeNz+KCcp4D8qhvOmOKMClRDdg==
     * 
     */
    @JsonProperty("rawValue")
    private String RawValue;
    @JsonIgnore
    private Map<String, Object> additionalProperties = new HashMap<String, Object>();

    /**
     *  2.5.29.14
     * 
     * @return
     *     The Oid
     */
    @JsonProperty("oid")
    public String getOid() {
        return Oid;
    }

    /**
     *  2.5.29.14
     * 
     * @param Oid
     *     The Oid
     */
    @JsonProperty("oid")
    public void setOid(String Oid) {
        this.Oid = Oid;
    }

    /**
     * Subject Key Identifier
     * 
     * @return
     *     The Name
     */
    @JsonProperty("name")
    public String getName() {
        return Name;
    }

    /**
     * Subject Key Identifier
     * 
     * @param Name
     *     The Name
     */
    @JsonProperty("name")
    public void setName(String Name) {
        this.Name = Name;
    }

    /**
     *  5E59E373F8A09CA780FCAA1BCE98E28C0A544376
     * 
     * @return
     *     The Value
     */
    @JsonProperty("value")
    public String getValue() {
        return Value;
    }

    /**
     *  5E59E373F8A09CA780FCAA1BCE98E28C0A544376
     * 
     * @param Value
     *     The Value
     */
    @JsonProperty("value")
    public void setValue(String Value) {
        this.Value = Value;
    }

    /**
     * BBReWeNz+KCcp4D8qhvOmOKMClRDdg==
     * 
     * @return
     *     The RawValue
     */
    @JsonProperty("rawValue")
    public String getRawValue() {
        return RawValue;
    }

    /**
     * BBReWeNz+KCcp4D8qhvOmOKMClRDdg==
     * 
     * @param RawValue
     *     The RawValue
     */
    @JsonProperty("rawValue")
    public void setRawValue(String RawValue) {
        this.RawValue = RawValue;
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
