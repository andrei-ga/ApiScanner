export interface NavMenuModel {
    pageTitle: string,
    embed?: boolean,
    links: NavMenuLinkModel[]
}

export interface NavMenuLinkModel {
    name: string,
    url: string
}