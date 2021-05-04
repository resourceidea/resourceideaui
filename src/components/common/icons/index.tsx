import React from "react";

type IconProps = {
  width?: number;
  height?: number;
  fill?: string;
  size?: number;
  children?: React.ReactChildren | React.ReactChild
};

const SVGShell = ({ size, width, height, children }: IconProps) => (
  <svg
    width={size ?? width ?? 24}
    height={size ?? height ?? 24}
    viewBox={`0 0 ${size ?? width ?? 24} ${size ?? height ?? 24}`}
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
  >
    {children}
  </svg>
)

export const ResourceIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M11.556 13c.796 0 1.444-.648 1.444-1.444V1.444C13 .648 12.352 0 11.556 0H1.444C.648 0 0 .648 0 1.444v10.112C0 12.352.648 13 1.444 13h10.112zM4.733 4.809l2.889 1.444 1.121-2.242 1.292.646-1.767 3.535-2.889-1.444L4.258 8.99l-1.293-.646 1.768-3.535z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>
);

export const MenuIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M5 6h13.5v1.44H5V6zm0 5.04h9v1.44H5v-1.44zm0 5.04h13.5v1.44H5v-1.44z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const ProfileIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <>
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M14.91 9.818a2.909 2.909 0 11-5.818 0 2.909 2.909 0 015.817 0zm-1.455 0a1.454 1.454 0 11-2.91 0 1.454 1.454 0 012.91 0z"
        fill={props.fill ?? '#333'}
      />
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M12 4a8 8 0 100 16 8 8 0 000-16zm-6.545 8c0 1.52.518 2.92 1.387 4.03a6.536 6.536 0 015.205-2.576 6.535 6.535 0 015.158 2.515A6.546 6.546 0 105.455 12zM12 18.546a6.519 6.519 0 01-4.125-1.464 5.085 5.085 0 014.172-2.173 5.084 5.084 0 014.138 2.124A6.52 6.52 0 0112 18.545z"
        fill={props.fill ?? '#333'}
      />
    </>
  </SVGShell>)

export const ChevronIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M.235.258c.323-.33.773-.357 1.168 0l2.898 2.778L7.198.258c.395-.357.846-.33 1.167 0 .324.33.303.887 0 1.197-.3.31-3.48 3.338-3.48 3.338a.812.812 0 01-1.17 0S.538 1.765.236 1.455c-.303-.31-.323-.867 0-1.197z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const ServiceIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M1.696.348v3.391H.566V.348h1.13zM.566 11.652h1.13V8.261H.566v3.391zM2.26 6A1.13 1.13 0 100 6a1.13 1.13 0 002.26 0zM13 2.609V9.39c0 .628-.503 1.13-1.13 1.13H5.087a1.13 1.13 0 01-1.13-1.13v-2.26L2.827 6l1.13-1.13V2.609a1.13 1.13 0 011.13-1.13h6.783c.627 0 1.13.502 1.13 1.13zm-2.826 3.956H5.652v1.13h4.522v-1.13zm1.13-2.26H5.652v1.13h5.652v-1.13z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const JobIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M12.316 2.737H9.579V1.368A1.368 1.368 0 008.21 0H5.474c-.76 0-1.369.609-1.369 1.368v1.369H1.368C.608 2.737 0 3.346 0 4.105v7.527A1.368 1.368 0 001.368 13h10.948a1.368 1.368 0 001.368-1.368V4.105a1.369 1.369 0 00-1.368-1.368zM5.474 1.368H8.21v1.369H5.474V1.368zM6.842 4.79a1.71 1.71 0 110 3.421 1.71 1.71 0 010-3.42zm3.421 6.843H3.421v-.856c0-.944 1.533-1.71 3.421-1.71 1.889 0 3.421.766 3.421 1.71v.856z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const ClientIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M1.444.5h10.112A1.444 1.444 0 0113 1.944v10.112a1.444 1.444 0 01-1.444 1.444H1.444A1.444 1.444 0 010 12.056V1.944A1.444 1.444 0 011.444.5zM2.89 3.389v1.444h1.444V3.39H2.89zm2.889 0v1.444h1.444V3.39H5.778zm2.889 0v1.444h1.444V3.39H8.667zM2.889 6.278v1.444h1.444V6.278H2.89zm2.889 0v1.444h1.444V6.278H5.778zm2.889 0v1.444h1.444V6.278H8.667zM2.889 9.167v1.444h1.444V9.167H2.89zm2.889 0v1.444h1.444V9.167H5.778zm2.889 0v1.444h1.444V9.167H8.667z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const PasswordIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M15 5h5v10h-5v2a1 1 0 001 1h2v2h-2.5c-.55 0-1.5-.45-1.5-1 0 .55-.95 1-1.5 1H10v-2h2a1 1 0 001-1V3a1 1 0 00-1-1h-2V0h2.5c.55 0 1.5.45 1.5 1 0-.55.95-1 1.5-1H18v2h-2a1 1 0 00-1 1v2zM0 5h11v2H2v6h9v2H0V5zm18 8V7h-3v6h3zM6.5 10a1.5 1.5 0 10-3 0 1.5 1.5 0 003 0zM11 8.89c-.61-.56-1.56-.51-2.12.11-.56.6-.51 1.55.12 2.11.55.52 1.43.52 2 0V8.89z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const EmailIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M17 0H3a3 3 0 00-3 3v10a3 3 0 003 3h14a3 3 0 003-3V3a3 3 0 00-3-3zm-.67 2L10 6.75 3.67 2h12.66zM17 14H3a1 1 0 01-1-1V3.25L9.4 8.8a1 1 0 001.2 0L18 3.25V13a1 1 0 01-1 1z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const IdIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <>
      <path d="M0 10v6h6v-2H2v-4H0z" fill={props.fill ?? '#333'} />
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="M5 5v6h6V5H5zm4 2H7v2h2V7z"
        fill={props.fill ?? '#333'}
      />
      <path
        d="M0 6V0h6v2H2v4H0zM16 6V0h-6v2h4v4h2zM16 10v6h-6v-2h4v-4h2z"
        fill={props.fill ?? '#333'}
      />
    </>
  </SVGShell>)

export const PhoneIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M17.11 13.086v2.43a1.62 1.62 0 01-1.765 1.62 16.033 16.033 0 01-6.991-2.486 15.797 15.797 0 01-4.86-4.86 16.031 16.031 0 01-2.487-7.024A1.62 1.62 0 012.619 1h2.43a1.62 1.62 0 011.62 1.393c.103.778.293 1.542.567 2.277a1.62 1.62 0 01-.365 1.709L5.843 7.408a12.961 12.961 0 004.86 4.86l1.029-1.029a1.62 1.62 0 011.71-.364c.734.274 1.498.464 2.275.567a1.62 1.62 0 011.394 1.644z"
      stroke={props.fill ?? '#333'}
      strokeWidth={2}
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </SVGShell>)

export const CompanyIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M20 16h2v2H0v-2h2V1a1 1 0 011-1h10a1 1 0 011 1v15h4V8h-2V6h3a1 1 0 011 1v9zM4 2v14h8V2H4zm2 6h4v2H6V8zm0-4h4v2H6V4z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const SettingsIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M13.827 6.179l-1.565-.448a5.1 5.1 0 00-.444-1.044l.766-1.376a.264.264 0 00-.051-.322l-1.117-1.075a.295.295 0 00-.336-.05l-1.425.73a5.556 5.556 0 00-1.102-.447L8.086.664a.275.275 0 00-.107-.135A.294.294 0 007.81.48H6.231c-.06 0-.12.02-.168.054a.274.274 0 00-.103.139L5.493 2.15c-.386.11-.76.26-1.111.448L2.98 1.873a.296.296 0 00-.336.05L1.509 2.984a.264.264 0 00-.051.323l.756 1.344a5.097 5.097 0 00-.467 1.062L.201 6.16a.285.285 0 00-.145.098A.266.266 0 000 6.42v1.515c0 .058.02.114.056.161a.285.285 0 00.145.099l1.555.448c.116.363.273.713.468 1.043l-.767 1.407a.264.264 0 00.052.323l1.116 1.07a.297.297 0 00.336.05l1.444-.74c.341.177.702.318 1.074.422l.467 1.51a.274.274 0 00.103.138.293.293 0 00.168.054h1.58c.06 0 .119-.02.168-.054a.274.274 0 00.102-.139l.467-1.514c.37-.104.727-.245 1.065-.421l1.453.744a.297.297 0 00.337-.05l1.116-1.07a.264.264 0 00.051-.323l-.775-1.389a5.09 5.09 0 00.444-1.026l1.574-.448a.285.285 0 00.145-.098.266.266 0 00.056-.161V6.443a.265.265 0 00-.043-.158.282.282 0 00-.13-.106zM7.02 9.664a2.648 2.648 0 01-1.428-.415 2.49 2.49 0 01-.946-1.106 2.37 2.37 0 01-.146-1.424c.1-.478.344-.917.703-1.261.36-.345.817-.58 1.316-.675a2.671 2.671 0 011.484.14c.47.187.87.503 1.153.908.282.405.433.882.433 1.369 0 .654-.27 1.28-.752 1.742a2.626 2.626 0 01-1.817.722z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const AdminsIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M4.9 6.427a2.82 2.82 0 001.556-.468 2.78 2.78 0 001.03-1.244 2.749 2.749 0 00-.606-3.023 2.825 2.825 0 00-3.051-.601c-.512.21-.95.565-1.257 1.022a2.755 2.755 0 00.348 3.501c.525.52 1.237.813 1.98.813zm5.6 1.386c.415 0 .821-.122 1.167-.35.345-.229.614-.554.773-.934a2.06 2.06 0 00-.455-2.266 2.119 2.119 0 00-2.289-.45 2.096 2.096 0 00-.942.765 2.066 2.066 0 00.261 2.626c.394.39.928.61 1.485.61zm2.8 4.854a.704.704 0 00.495-.203.69.69 0 00.205-.49c0-.649-.184-1.284-.53-1.833a3.488 3.488 0 00-1.43-1.278 3.53 3.53 0 00-3.682.372A4.915 4.915 0 005.85 7.912a4.944 4.944 0 00-2.827.278A4.89 4.89 0 00.827 9.975 4.82 4.82 0 000 12.667c0 .184.074.36.205.49s.31.203.495.203h8.4a.703.703 0 00.495-.203.69.69 0 00.205-.49"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)

export const InfoIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <path
      d="M14 7c0-3.87-3.14-7-7-7-3.87 0-7 3.13-7 7s3.13 7 7 7c3.86 0 7-3.13 7-7zM7.7 8.48H6.14v-.43c0-.38.08-.7.24-.98.16-.28.46-.57.88-.89.41-.29.68-.53.81-.71.14-.18.2-.39.2-.62 0-.25-.09-.44-.28-.58-.19-.13-.45-.19-.79-.19-.58 0-1.25.19-2 .57l-.64-1.28c.87-.49 1.8-.74 2.77-.74.81 0 1.45.2 1.92.58.48.39.71.91.71 1.55 0 .43-.09.8-.29 1.11-.19.32-.57.67-1.11 1.06-.38.28-.61.49-.71.63-.1.15-.15.34-.15.57v.35zm-1.47 2.74c-.18-.17-.27-.42-.27-.73 0-.33.08-.58.26-.75.18-.17.43-.25.77-.25.32 0 .57.09.75.26.18.17.27.42.27.74 0 .3-.09.55-.27.72-.18.18-.43.27-.75.27-.33 0-.58-.09-.76-.26z"
      fill={props.fill ?? '#333'}
    />
  </SVGShell>)


export const LogoIcon = (props: IconProps) => (
  <SVGShell {...props}>
    <>
      <mask
        id="prefix__a"
        maskUnits="userSpaceOnUse"
        x={0}
        y={0}
        width={20}
        height={20}
      >
        <path fill="url(#prefix__pattern0)" d="M0 0h20v20H0z" />
      </mask>
      <g mask="url(#prefix__a)">
        <path fill="#fff" d="M0 0h20v20H0z" />
      </g>
      <defs>
        <pattern
          id="prefix__pattern0"
          patternContentUnits="objectBoundingBox"
          width={1}
          height={1}
        >
          <use xlinkHref="#prefix__image0" transform="scale(.005)" />
        </pattern>
        <image
          id="prefix__image0"
          width={200}
          height={200}
          xlinkHref="data:image/png;base64,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"
        />
      </defs>
    </>
  </SVGShell>)