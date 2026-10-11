#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

if ! command -v nix >/dev/null 2>&1; then
    echo "Nix is required to run Ryujinx on this system." >&2
    exit 127
fi

# Codex's managed runtime may not expose an ALSA/PulseAudio device. SDL treats
# that as a fatal initialization error, so use its null backend only there.
if [ ! -d /dev/snd ] && [ -z "${SDL_AUDIODRIVER:-}" ]; then
    export SDL_AUDIODRIVER=dummy
fi

# The Avalonia and SkiaSharp native assets need the runtime library path set by
# the Nix development shell before the .NET process starts. `path:.` also lets
# this work before flake.nix has been added to Git.
exec nix develop path:. --command dotnet run --project src/Ryujinx/Ryujinx.csproj --configuration Debug -- "$@"
