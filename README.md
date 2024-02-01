# PoloTweaks

[![Plugin builds](https://github.com/NotNite/PoloTweaks/actions/workflows/plugin.yml/badge.svg)](https://github.com/NotNite/PoloTweaks/actions/workflows/plugin.yml)

Bomb Rush Cyberfunk tweaks.

## Config

> [!WARNING]
> This mod uses a custom config system that will not work with r2modman. You are expected to edit the config file by hand in a text editor. I am not going to help you if you struggle with the TOML syntax - it is heavily documented on the internet.

Create a config file named  `PoloTweaks.toml` in your BepInEx config directory. The config is split into modules. Each module has an ID associated with it, an enabled toggle, and the config options (if any).

### `fov`

Allows you to change your FOV.

```toml
[fov]
enabled = false
fov = 90
```

### `trail`

Sticks trail renderers onto various bones on the player.

```toml
[trail]
enabled = false
all_players = false # Show trails on other players if using Slop Crew

# By default there are no trails - you can add more like this:
[[trail.trails]]
bone = "footl"
color = "#FF0000D9"
length = 0.5

[[trail.trails]]
bone = "footr"
color = "#FF0000D9"
length = 0.5
```
