# KRelay
## RotMG x4.2.1.0.0
### A modular Realm of the Mad God man-in-the-middle Proxy

![Screenshot](/NewScreenshot.png)
-----------------------------------------------------------

## Setting Up (Assuming you have a compiled binary)
1. Open K Relay and Exalt
2. Connect to USSW2 (the proxy server)

## Updated Things
1. Now uses a custom dll that injects into the game, and hooks the connect function to proxy to localhost. [Can be found here](https://github.com/Cursed-Gato/KRelayProxy)
2. Fixed most of the old packets to be read correctly, some I think are wrong, I think thats why its having issues loading into realms and showing daily quests.
3. Added most outgoing packets from the game files, I think between 5-10 are not implemented. Most new server packets are still not implemented.
4. Client Time has been changed but im not very happy with it, need to find a better way to sync it.

## TODO
1. Create or steal a object.xml and tile.xml extractor, mine is clapped for some reason.
2. Fix client time Sync.
3. Implement the remaining packets.
4. Add some kinda of documentation for Qol tools for manipulating packets. 

## Plugin Documentation (OLD)
[Can be found here](https://github.com/TheKronks/K_Relay_Plugin_Documentation/blob/master/README.md)
