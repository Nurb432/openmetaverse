# Facebook #

Three different Facebook social features are enabled in the web frontend. Facebook Connect may be used for authentication at login and registration. When registering, the users name and email will be suggested. Region and user pages will have 'like' buttons and the about page will have a listing of recent 'likes'. In addition to the _use\_facebook_ configurable option being enabled, the _facebook\_secret_ and _facebook\_id_ must be set. These values may be retrieved from the Facebook application settings page and are _Application ID_ and _Application Secret_ respectively. In addition, the _Site URL_ must be configured under the _Web Site_ section of the Facebook application settings page.

# Twitter #

Twitter integration is currently limited to basic 'tweet' buttons in the web front end. These will show up in region and user pages if the _use\_twitter_ configuration option is enabled.

# OpenID #

A variety of OpenID providers may be used for authentication for login and registration when the _openid\_enabled_ configuration option is enabled. When registering, the users name and email will be suggested.