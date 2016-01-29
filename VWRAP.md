**NOTE: This page is not an authoritative resource for the VWRAP IETF group. The protocols described here are a work in progress, and do not reflect a consensus of implementation or even scope of the VWRAP working group.**

**NOTE 2: This page is a work in progress. The protocol messages documented here are incomplete and not implemented anywhere yet.**

### Public Region Seed Capability ###

```
%% public_region_seed_cap -> undef <- &resp
 
; success 
&resp = {
           capabilities:
           [
              rez_avatar/request: uri,
              child_avatar/update: uri,
              region/online: uri,
              region/offline: uri
           ]
        }

; error
&resp = {
           message: string
        }
```

### Rez Avatar Request ###

```
%% rez_avatar/request -> {
                            agent_id: uuid,
                            circuit_code: int,
                            secure_session_id: uuid,
                            session_id: uuid,
                            first_name: string,
                            last_name: string,
                            position: [ real, real, real ],             ; optional
                            access_level: integer,
                            child: bool
                         }

 ; successful response
                      <- {
                            connect: true,
                            region_seed_capability: uri,
                            sim_host: string,
                            sim_port: int,

                         ; Note: These fields are region information
                            region_id: uuid,
                            region_x: int,
                            region_y: int
                         }

; general failure
                      <- {
                            connect: false,
                            message: string
                         }
```

### Child Avatar Update ###

```
%% child_avatar/update -> {
                             agent_id: uuid,
                             position: [ real, real, real ],
                             rotation: [ real, real, real, real ],
                             camera_center: [ real, real, real ],
                             camera_at: [ real, real, real ],
                             camera_left: [ real, real, real ],
                             camera_up: [ real, real, real ],
                             draw_distance: real
                          }
```

### Region Online ###

```
%% region/online -> {
                       region_id: uuid,
                       region_name: string,
                       region_x: int,
                       region_y: int,
                       public_region_seed_capability: uri,
                       sim_host: string,
                       sim_port: int
                    }
```

### Region Offline ###

```
%% region/offline -> {
                        region_id: uuid
                     }
```