import { ofType } from 'redux-observable';
import { mergeMap} from 'rxjs/operators';
import { from, of } from 'rxjs';
import { updateId } from '../redux/categorySlice';
import { getOptions } from './options';

export const sendMutation = (query, currentDatabase) => {
    return fetch(getOptions().apiEndPoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'CurrentDatabase': currentDatabase
      },
      body: JSON.stringify({
        query: query
      })
    })
};

export const addCategoryEpic = (action$, state$) =>
    action$.pipe(
      ofType('categories/addCategory'),
      mergeMap(action =>
        from(sendMutation(
        ` mutation {
                categories_M {
                    add(name: "${action.payload.name}") {
                        id
                    }
                }
            }
        `, state$.value.database
        )).pipe(
            mergeMap(response => response.json()),
            mergeMap((data) => {
                return of(updateId({
                    oldId: action.payload.id,
                    newId: data.data.categories_M.add.id
                }));
            })
          )
        )
);

export const deleteCategoryEpic = (action$, state$) =>
    action$.pipe(
      ofType('categories/deleteCategory'),
      mergeMap(action =>
        from(sendMutation(
        ` mutation {
                categories_M {
                    delete(id: ${action.payload.id}) {
                        id
                    }
                }
            }
        `, state$.value.database
        )).pipe(
          mergeMap(() => {
              return of();
          })
        )
      )
);