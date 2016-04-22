
package bobs.paraf.web;

import java.util.HashMap;
import java.util.Map;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonValue;

@Generated("org.jsonschema2pojo")
public enum Keyusage {

    NONREPUDIATION("nonrepudiation");
    private final String value;
    private final static Map<String, Keyusage> CONSTANTS = new HashMap<String, Keyusage>();

    static {
        for (Keyusage c: values()) {
            CONSTANTS.put(c.value, c);
        }
    }

    private Keyusage(String value) {
        this.value = value;
    }

    @JsonValue
    @Override
    public String toString() {
        return this.value;
    }

    @JsonCreator
    public static Keyusage fromValue(String value) {
        Keyusage constant = CONSTANTS.get(value);
        if (constant == null) {
            throw new IllegalArgumentException(value);
        } else {
            return constant;
        }
    }

}
