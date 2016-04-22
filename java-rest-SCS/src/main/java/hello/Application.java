package hello;

import java.util.List;
import org.eclipse.jetty.server.NetworkTrafficServerConnector;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.Banner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.embedded.jetty.JettyEmbeddedServletContainerFactory;
import org.springframework.boot.context.embedded.jetty.JettyServerCustomizer;
import org.springframework.context.annotation.Bean;
import org.eclipse.jetty.server.Server;

@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        //   SpringApplication.run(Application.class, args);
        SpringApplication app = new SpringApplication(Application.class);
        app.setBannerMode(Banner.Mode.OFF);

        app.run(args);
    }

    @Bean
    public JettyEmbeddedServletContainerFactory jettyEmbeddedServletContainerFactory(
            @Value("${server.port:8443}") final String mainPort,
            @Value("#{'${server.secondary.ports}'.split(',')}") final List<String> secondaryPorts) {

        final JettyEmbeddedServletContainerFactory factory = new JettyEmbeddedServletContainerFactory(Integer.valueOf(mainPort));

        // Add customized Jetty configuration with non blocking connection handler
        factory.addServerCustomizers(new JettyServerCustomizer() {
            @Override
            public void customize(final Server server) {
                // Register an additional connector for each secondary port.
                for (final String secondaryPort : secondaryPorts) {
                    final NetworkTrafficServerConnector connector = new NetworkTrafficServerConnector(server);
                    connector.setPort(Integer.valueOf(secondaryPort));
                    server.addConnector(connector);
                }

                // Additional configuration
            }

        });
        return factory;
    }
}
