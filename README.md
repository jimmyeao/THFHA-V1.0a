
![example workflow](https://github.com/jimmyeao/THFHA-V1.0a/actions/workflows/dotnet.yml/badge.svg)
# Teams Helper - BETA1!

This project started out as way to get Microsoft Teams status and activity updates into home assistant, it has since grown to provide standalone connection to Philips Hue lights, WLED lights, out [own hatcher project](https://github.com/jimmyeao/Hatch_Companion) and MQTT.

Originally, all updates were parsed from the logs that Teams creates, today we only get status updates from the logs, all other activity comes from the new Teams local API (Thanks Microsoft!)

I am hoping Microsoft add status updates to the local api as well, as constantly readying a log file is not very efficient.

The whole thing is written in C# and is pretty lightweight.

# Features:

- Light state (Hue, WLED and Hatcher) are restored to their original state on exit, stop or if the module is disabled
- Minimises to system tray to keep your task bar clutter free
- Can run independently of Home Assistant
- Publish updates to MQTT
- Change the color of WLED / Hue devices dependant on Status/Activity

# First Run:

![](RackMultipart20230312-1-33g1ip_html_9353a7e229d8c12f.png)

On first run, nothing is configured, but it will start monitoring the Teams logs in the background, so your Status icon may be different, depending on your current Teams status

First step is to go to Teams, open settings, privacy, scroll to Third Party API and click manage API:

![image](https://user-images.githubusercontent.com/5197831/224551585-f285419c-435d-4ce6-997a-35266194b8a9.png)

Enable the API and copy the key.

In THFHA go to File – Settings and paste in your key (It is automatically saved):
![image](https://user-images.githubusercontent.com/5197831/224551602-0535fbfc-98e2-444a-8e6e-d09929602bfe.png)

Also note the two options at the bottom:

Run at Startup does NOT make the app launch when you start windows, rather it starts controlling lights/mqtt etc when the app is run.

Run minimised does exactly what it says, an it will appear in your system tray.

# Home Assistant

To configure Home Assistant, you can either double click the Home Assistant entry in the available modules list, or you can access its settings page from File \> Settings:

Enter the URL to your home assistant instance (Including the port it it is not 443)

e.g

[http://homeassistant.local:8123](http://homeassistant.local:8123/)

or if you are using ssl:

[https://homeassistant.local](https://homeassistant.local/)

you will need a long lived access token from Home assistant (See [here](https://developers.home-assistant.io/docs/auth_api/#:~:text=Long%2Dlived%20access%20tokens%20can,access%20token%20for%20current%20user.))

Paste it into the box as shown:

![image](https://user-images.githubusercontent.com/5197831/224551626-41e4e2b6-9f01-4afc-bb31-bf4b420038c0.png)

Click Test, if all is OK you will see "Homeassistant is connected" in the status bar. If you get an error, please check the url, port, scheme (http or https) and token.

You should now be able to enable this either by right clicking:

![image](https://user-images.githubusercontent.com/5197831/224551646-67484e92-fa5a-4560-8313-3605db5b2b5d.png)

Or in the settings:

![image](https://user-images.githubusercontent.com/5197831/224551655-23d98e1b-c4b9-41cf-b5e0-2816f19ff21e.png)

Once enabled, (And the app is started)

You should see new sensors created in Home assistant with the prefix THFHA:

![image](https://user-images.githubusercontent.com/5197831/224551668-343e7c86-793f-4bd1-b023-74ff3606e040.png)

These should be pretty self explanatory

Activity = generally "Not in a Meeting" or "In a Meeting"

Blurred – indicate whether or not your back ground is blurred

Camera – On/Off state of Camera

Hand – whether or not you have your hand raised in a meeting

Microphone – mute state

Recording – whether or not the call is being recorded

Status – Available/Busy/DND/Be Right Back/Away/Offline etc.

# Philips Hue

![image](https://user-images.githubusercontent.com/5197831/224551678-2cbf83d0-0ae2-4fbe-9f47-5d93ed5e909c.png)

Enter the ip of your hue bridge and click Link

![image](https://user-images.githubusercontent.com/5197831/224551681-9c886859-ce7a-4372-b42a-b64cbed03c68.png)

you will be prompted to push the link button on your hub – please do so before proceeding to click ok!

![image](https://user-images.githubusercontent.com/5197831/224551693-4bb4729d-ea96-4b73-aa68-16b711cb30ea.png)

Once linked, your available hue lights will appear in the list. Choose the one you wish to control (Currently, only one light may be selected)


![image](https://user-images.githubusercontent.com/5197831/224551702-ae3a60ac-dee1-4008-95b9-18eaf7086094.png)


Once again, right click in the module list to enable Hue control.

# MQTT

Double click MQTT to open its settings:

![image](https://user-images.githubusercontent.com/5197831/224551729-6aebd221-4e11-4f5f-ae55-354173f77269.png)

Note, you don't need to specify the port for mqtt unless you are using something other than the default (untested)

If you have a username and password, enter these

MQTT Topic is the prefix that the topic will start with.

Click Test:

![image](https://user-images.githubusercontent.com/5197831/224551740-8a799d52-fc8f-4561-b23a-60cf0c672f19.png)

If you receive an error, please double check your settings.

This will now be publishing to MQTT under the main topic of HomeAssistant, and should create the sensors for you based on your MQTT Topic: (Mine here was ryzen, ignore the ryzen\_lastboot, that is from a different integration!)
![image](https://user-images.githubusercontent.com/5197831/224551749-43edabf0-8898-42e6-819c-8fa308d4b1bf.png)

You can now enable MQTT by right clicking it in the module list.

# WLED

WLED relies on mDns for discovery, it is usually enabled by default on most home routers, but some prosumer equipment (Unifi etc) may need mDns enabling.

![image](https://user-images.githubusercontent.com/5197831/224551756-d931aa58-ad6c-4038-aeb6-cbfffb5e82a9.png)

Click discover to have the program discover your devices, please wait until the status bar shows "Stopped Discovering Lights"

Select your WLED light from the list:
 ![image](https://user-images.githubusercontent.com/5197831/224551768-d979980c-c64d-4f9a-8612-5a9e3d0ce27c.png)

Once again, right click to enable in the modules list

# Hatcher

![image](https://user-images.githubusercontent.com/5197831/224551781-85caae93-3d85-4a62-a2f4-6669c5cd8d31.png)

We built a device based on a Raspberry Pi zero and a circular LED display – see

[https://github.com/jimmyeao/Hatch\_Companion](https://github.com/jimmyeao/Hatch_Companion)

it is possible to control this from THFHA. Simply enter the IP of your Pi and click test:

![image](https://user-images.githubusercontent.com/5197831/224551792-e0e45c57-1641-4500-813e-ea0f6c74e4df.png)

If all is well, you will now be able to control this device from THFHA
