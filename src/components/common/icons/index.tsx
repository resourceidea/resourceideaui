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