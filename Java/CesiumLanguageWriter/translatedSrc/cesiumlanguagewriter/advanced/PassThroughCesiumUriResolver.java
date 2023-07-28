package cesiumlanguagewriter.advanced;


import agi.foundation.compatibility.*;
import cesiumlanguagewriter.*;
import javax.annotation.Nonnull;

/**
 * A URI resolver that leaves URIs unchanged.
 */
@SuppressWarnings({
    "unused",
    "deprecation",
    "serial"
})
public class PassThroughCesiumUriResolver implements ICesiumUriResolver {

    /**
     * Resolves a URI, leaving it unchanged.
     *
     * @param uri The source URI.
     * @return The same URI.
     */
    public String resolveUri(String uri) {
        return uri;
    }

    /**
     * Gets a static instance of {@link PassThroughCesiumUriResolver} usable from any thread.
     */
    public static final PassThroughCesiumUriResolver INSTANCE = new PassThroughCesiumUriResolver();
}