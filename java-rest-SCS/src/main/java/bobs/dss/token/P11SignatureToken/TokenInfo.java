
package bobs.dss.token.P11SignatureToken;

import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Generated("org.jsonschema2pojo")
@JsonPropertyOrder({
    "slotID",
    "ckaLabel",
    "label",
    "manufacturerId",
    "model",
    "serialNumber"
})
public class TokenInfo {

    @JsonProperty("slotID")
    private Integer SlotID;
    @JsonProperty("ckaLabel")
    private String ckaLabel;
    @JsonProperty("label")
    private String Label;
    @JsonProperty("manufacturerId")
    private String ManufacturerId;
    @JsonProperty("model")
    private String Model;
    @JsonProperty("serialNumber")
    private String SerialNumber;

    /**
     * 
     * @return
     *     The SlotID
     */
    @JsonProperty("slotID")
    public Integer getSlotID() {
        return SlotID;
    }

    /**
     * 
     * @param SlotID
     *     The SlotID
     */
    @JsonProperty("slotID")
    public void setSlotID(Integer SlotID) {
        this.SlotID = SlotID;
    }

    public TokenInfo withSlotID(Integer SlotID) {
        this.SlotID = SlotID;
        return this;
    }

    /**
     * 
     * @return
     *     The ckaLabel
     */
    @JsonProperty("ckaLabel")
    public String getCkaLabel() {
        return ckaLabel;
    }

    /**
     * 
     * @param ckaLabel
     *     The ckaLabel
     */
    @JsonProperty("ckaLabel")
    public void setCkaLabel(String ckaLabel) {
        this.ckaLabel = ckaLabel;
    }

    public TokenInfo withCkaLabel(String ckaLabel) {
        this.ckaLabel = ckaLabel;
        return this;
    }

    /**
     * 
     * @return
     *     The Label
     */
    @JsonProperty("label")
    public String getLabel() {
        return Label;
    }

    /**
     * 
     * @param Label
     *     The Label
     */
    @JsonProperty("label")
    public void setLabel(String Label) {
        this.Label = Label;
    }

    public TokenInfo withLabel(String Label) {
        this.Label = Label;
        return this;
    }

    /**
     * 
     * @return
     *     The ManufacturerId
     */
    @JsonProperty("manufacturerId")
    public String getManufacturerId() {
        return ManufacturerId;
    }

    /**
     * 
     * @param ManufacturerId
     *     The ManufacturerId
     */
    @JsonProperty("manufacturerId")
    public void setManufacturerId(String ManufacturerId) {
        this.ManufacturerId = ManufacturerId;
    }

    public TokenInfo withManufacturerId(String ManufacturerId) {
        this.ManufacturerId = ManufacturerId;
        return this;
    }

    /**
     * 
     * @return
     *     The Model
     */
    @JsonProperty("model")
    public String getModel() {
        return Model;
    }

    /**
     * 
     * @param Model
     *     The Model
     */
    @JsonProperty("model")
    public void setModel(String Model) {
        this.Model = Model;
    }

    public TokenInfo withModel(String Model) {
        this.Model = Model;
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

    public TokenInfo withSerialNumber(String SerialNumber) {
        this.SerialNumber = SerialNumber;
        return this;
    }

}
