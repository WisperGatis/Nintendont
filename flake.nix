{
  description = "Ryujinx .NET development environment";

  inputs.nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";

  outputs = { nixpkgs, ... }:
    let
      eachSystem = nixpkgs.lib.genAttrs [ "x86_64-linux" "aarch64-linux" ];
    in
    {
      devShells = eachSystem (system:
        let
          pkgs = import nixpkgs { inherit system; };
          runtimeLibraries = with pkgs; [
            SDL2
            libsoundio
            openal
            vulkan-loader
            libGL
            libdrm
            libxkbcommon
            fontconfig
            libx11
            libxcursor
            libxext
            libxi
            libxrandr
            libxrender
            libxcb
            libice
            libsm
          ];
        in
        {
          default = pkgs.mkShell {
            packages = with pkgs; [
              dotnet-sdk_10
              pkg-config
              ffmpeg
            ] ++ runtimeLibraries;

            shellHook = ''
              # NuGet native assets are not patched with Nix store paths.
              export LD_LIBRARY_PATH="${pkgs.lib.makeLibraryPath runtimeLibraries}''${LD_LIBRARY_PATH:+:$LD_LIBRARY_PATH}"
              export DOTNET_CLI_TELEMETRY_OPTOUT=1
              export DOTNET_NOLOGO=1
            '';
          };
        });
    };
}
