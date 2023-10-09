import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {URLS} from "./shared/urls.constants";

@Injectable({
  providedIn: 'root'
})
export class GitHubService {

  constructor(private http: HttpClient) { }

  public getRepositories(): Observable<IRepository[]> {
    return this.http.get<IRepository[]>(URLS.REPOSITORIES);
  }
}
