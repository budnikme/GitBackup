import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {URLS} from './shared/urls.constants';

@Injectable({
  providedIn: 'root'
})
export class BackupsService {

  constructor(private http: HttpClient) {
  }

  public create(repositoryId: number): Observable<void> {
    return this.http.post<void>(URLS.BACKUPS(repositoryId), {});
  }

  public restore(id: number): Observable<void> {
    return this.http.post<void>(URLS.RESTORE(id), {});
  }
}
